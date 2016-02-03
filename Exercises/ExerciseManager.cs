namespace HSA.RehaGame.Exercises
{
    using DB;
    using Input.Kinect;
    using InGame;
    using Scene;
    using UI;
    using UI.VisualExercise;
    using UnityEngine;
    using UnityEngine.UI;
    using User;
    using FulFillables;
    using Kinect = Windows.Kinect;
    using System;

    [RequireComponent(typeof(AudioSource))]
    public class ExerciseManager : MonoBehaviour
    {
        public GameObject DrawingPrefab;
        public GameObject MenuPrefab;
        public GameObject waitPanel;

        private MovieTexture movieTexture;
        private AudioSource audioSource;
        private Drawing drawing;

        private SwapCanvas swapCanvas;

        private Kinect.Body body;
        private Exercise exercise;
        private Patient patient;
        private RGMLManager relManager;
        private ExerciseExecutionManager executionManager;

        private bool hasUser;
        private bool exerciseRuns;
        private bool isFullfilled;

        private double startTime;
        private double now;

        // Use this for initialization
        void Start()
        {
            drawing = DrawingPrefab.GetComponent<Drawing>();

            waitPanel.GetComponentInChildren<Text>().text = DBManager.GetTranslation("noUser").GetValueFromLanguage("translation");
            swapCanvas = MenuPrefab.GetComponent<SwapCanvas>();

            patient = GameState.ActivePatient;
            exercise = GameState.ActiveExercise;

            movieTexture = exercise.Video;
            movieTexture.loop = true;

            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = exercise.AuditiveDescription;

            GameObject.Find("ExerciseHeader").GetComponent<Text>().text = exercise.Name;

            GameObject.Find("Video").GetComponent<RawImage>().texture = movieTexture;
            GameObject.Find("Description").GetComponentInChildren<Text>().text = exercise.Description;
            GameObject.Find("Information").GetComponent<Text>().text = exercise.Information;
        }

        void Update()
        {
            if (body != null && GameObject.Find(GameState.ActivePatient.Name) != null)
            {
                GameState.HasKinectUser = hasUser = true;
                waitPanel.SetActive(false);

                if (exerciseRuns)
                {
                    now = DateTime.Now.TimeOfDay.TotalSeconds;
                    isFullfilled = executionManager.IsFulfilled(body);

                    if (isFullfilled)
                    {
                        exerciseRuns = false;
                        GameState.ExecutionTime = now - startTime;

                        BodySourceManager.ShutdownKinect();
                        LoadScene.LoadStatistics();
                    }
                    else
                    {
                        executionManager.Write(body);
                    }
                }
            }
            else
            {
                if (exerciseRuns)
                    LoadScene.LoadExercise();

                waitPanel.SetActive(true);
                hasUser = false;
            }
        }

        public void StartExercise()
        {
            if (hasUser)
            {
                swapCanvas.SwapVidibility();
                this.StopMedia();

                if (relManager == null)
                {
                    this.relManager = new RGMLManager(patient, drawing, exercise.REL);
                    this.executionManager = new ExerciseExecutionManager(this.relManager.ParseRGML(), exercise.StressedJoints, drawing, null);
                }

                GameState.ExerciseIsActive = exerciseRuns = true;
                startTime = DateTime.Now.TimeOfDay.TotalSeconds;
            }
        }

        public void StopExercise()
        {
            GameState.ExerciseIsActive = exerciseRuns = false;
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
            audioSource.Stop();
        }

        public void ReadDescription()
        {
            bool justStopped = false;

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                justStopped = true;
            }

            if (RGSettings.reading && (!justStopped || audioSource.clip != exercise.AuditiveDescription))
            {
                audioSource.clip = exercise.AuditiveDescription;
                audioSource.Play();
            }
        }

        public void ReadInformation()
        {
            bool justStopped = false;

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                justStopped = true;
            }

            if (RGSettings.reading && (!justStopped || audioSource.clip != exercise.AuditiveInformation))
            {
                audioSource.clip = exercise.AuditiveInformation;
                audioSource.Play();
            }
        }

        public Kinect.Body Body
        {
            get
            {
                return body;
            }

            set
            {
                body = value;
            }
        }
    }
}