namespace HSA.RehaGame.Manager
{
    using Audio;
    using DB.Models;
    using Exercises.FulFillables;
    using Input.Kinect;
    using System.Collections.Generic;
    using UI;
    using UI.AuditiveExercise.PitchSounds;
    using UI.Feedback;
    using UnityEngine;
    using UnityEngine.UI;
    using Windows.Kinect;

    public class ExerciseManager : MonoBehaviour
    {
        public GameObject gameManager;
        public GameObject drawingPrefab;
        public GameObject waitPanel;
        public GameObject menuTitle;
        public GameObject menuContainer;

        private IDictionary<object, Exercise> models;

        private MovieTexture movieTexture;

        private SettingsManager settingsManager;
        private SoundManager soundManager;
        private SceneManager sceneManager;
        private BodyManager bodyManager;

        private Feedback feedback;

        private SwapCanvas swapCanvas;

        private Body body;
        private Exercise exercise;
        private PatientManager patientManager;
        private REMLManager relManager;
        private ExerciseExecutionManager executionManager;

        private FulFillable firstFulFillable;

        private bool hasUser;
        private bool exerciseRuns;
        private bool isFullfilled;

        private double startTime;
        private double now;

        // Use this for initialization
        void Start()
        {
            feedback = drawingPrefab.GetComponent<Feedback>();
            swapCanvas = drawingPrefab.GetComponent<SwapCanvas>();
            settingsManager = gameManager.GetComponent<SettingsManager>();
            patientManager = gameManager.GetComponent<PatientManager>();
            sceneManager = gameManager.GetComponent<SceneManager>();
            soundManager = gameManager.GetComponentInChildren<SoundManager>();
            bodyManager = this.GetComponentInChildren<BodyManager>();

            bodyManager.BodyDetected += BodyManager_BodyDetected;
            bodyManager.BodyLost += BodyManager_BodyLost;

            waitPanel.GetComponentInChildren<Text>().text = Model.GetModel<ValueTranslation>("noUser").Translation;

            exercise = GameManager.ActiveExercise;
            patientManager.SetStressedJoints(exercise);

            movieTexture = exercise.Video;
            movieTexture.loop = true;

            menuTitle.GetComponentInChildren<Text>().text = exercise.Name;

            GameObject.Find("Video").GetComponent<RawImage>().texture = movieTexture;
            GameObject.Find("Description").GetComponentInChildren<Text>().text = exercise.Description;
            GameObject.Find("Information").GetComponent<Text>().text = exercise.Information;

        }

        private void BodyManager_BodyLost()
        {
            if (this.body != null)
            {
                this.body = null;

                if (exerciseRuns)
                    sceneManager.LoadExercise();

                waitPanel.SetActive(true);
                GameManager.HasKinectUser = hasUser = false;
            }
        }

        private void BodyManager_BodyDetected(Body body)
        {
            if (this.body == null)
            {
                this.body = body;

                waitPanel.SetActive(false);
                GameManager.HasKinectUser = hasUser = true;
                feedback.VisualizePatient();
            }
        }

        void Update()
        {
            if (body != null && GameObject.Find(patientManager.ActivePatient.Name) != null)
            {
                if (exerciseRuns)
                {
                    isFullfilled = executionManager.IsFulfilled(body);

                    executionManager.Clear();
                    executionManager.Draw(body);
                    executionManager.Write(body);
                    executionManager.PlayValue();

#if UNITY_EDITOR
                    executionManager.Debug(body, exercise.StressedJoints);
#endif

                    if (isFullfilled)
                    {
                        exerciseRuns = false;
                        firstFulFillable.SetEndTime();

                        GameManager.ExecutionTimes = executionManager.GetExecutionTimes();
                        BodySourceManager.ShutdownKinect();
                        sceneManager.LoadStatistics();
                    }
                }
            }
        }

        public void StartExercise()
        {
            if (hasUser)
            {
                swapCanvas.SwapVisibility();
                this.StopMedia();

                if (relManager == null)
                {
                    this.relManager = new REMLManager(patientManager.ActivePatient, settingsManager, feedback, exercise.Reml);
                    this.firstFulFillable = this.relManager.ParseRGML();
                    this.executionManager = new ExerciseExecutionManager(this.firstFulFillable as BaseStep, settingsManager, feedback, PitchType.pitchDefault, relManager.Name, null);
                }

                GameManager.ExerciseIsActive = exerciseRuns = true;
                firstFulFillable.SetStartTime();
            }
        }

        public void StopExercise()
        {
            GameManager.ExerciseIsActive = exerciseRuns = false;
        }

        public void PlayVideo()
        {
            if (movieTexture.isPlaying)
                movieTexture.Stop();
            else
                movieTexture.Play();
        }

        public void StopMedia()
        {
            movieTexture.Stop();
            soundManager.Stop();
        }

        public void ReadDescription()
        {
            bool justStopped = false;

            if (soundManager.isPlaying)
            {
                soundManager.Stop();
                justStopped = true;
            }

            if (settingsManager.GetValue<bool>("ingame", "reading") && (!justStopped || exercise.AuditiveDescription))
            {
                soundManager.Enqueue(exercise.AuditiveDescription);
            }
        }

        public void ReadInformation()
        {
            bool justStopped = false;

            if (soundManager.isPlaying)
            {
                soundManager.Stop();
                justStopped = true;
            }

            if (settingsManager.GetValue<bool>("ingame", "reading") && (!justStopped || exercise.AuditiveInformation))
            {
                soundManager.Enqueue(exercise.AuditiveInformation);
            }
        }
    }
}