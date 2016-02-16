namespace HSA.RehaGame.Manager
{
    using System;
    using DB;
    using DB.Models;
    using Input.Kinect;
    using UI;
    using UI.VisualExercise;
    using UnityEngine;
    using UnityEngine.UI;
    using Kinect = Windows.Kinect;

    [RequireComponent(typeof(AudioSource))]
    public class ExerciseManager : BaseModelManager<Exercise>
    {
        public GameObject gameManager;
        public GameObject drawingPrefab;
        public GameObject waitPanel;

        private MovieTexture movieTexture;
        private AudioSource audioSource;

        private Database database; // ToDo Datanbank entfernen
        private Settings settings;
        private SceneManager sceneManager;

        private Drawing drawing;

        private SwapCanvas swapCanvas;

        private Kinect.Body body;
        private Exercise exercise;
        private PatientManager patientManager;
        private REMLManager relManager;
        private ExerciseExecutionManager executionManager;

        private bool hasUser;
        private bool exerciseRuns;
        private bool isFullfilled;

        private double startTime;
        private double now;

        // Use this for initialization
        void Start()
        {
            drawing = drawingPrefab.GetComponent<Drawing>();
            swapCanvas = drawingPrefab.GetComponent<SwapCanvas>();
            database = gameManager.GetComponent<Database>();
            settings = gameManager.GetComponent<Settings>();
            patientManager = gameManager.GetComponent<PatientManager>();
            sceneManager = gameManager.GetComponent<SceneManager>();

            waitPanel.GetComponentInChildren<Text>().text = Model.GetModel<ValueTranslation>("noUser").Translation;

            exercise = GameManager.ActiveExercise;

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
            if (body != null && GameObject.Find(patientManager.ActivePatient.Name) != null)
            {
                GameManager.HasKinectUser = hasUser = true;
                waitPanel.SetActive(false);

                if (exerciseRuns)
                {
                    now = DateTime.Now.TimeOfDay.TotalSeconds;
                    isFullfilled = executionManager.IsFulfilled(body);

                    if (isFullfilled)
                    {
                        exerciseRuns = false;
                        GameManager.ExecutionTime = now - startTime;

                        BodySourceManager.ShutdownKinect();
                        sceneManager.LoadStatistics();
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
                    sceneManager.LoadExercise();

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
                    this.relManager = new REMLManager(patientManager.ActivePatient, database, settings, drawing, exercise.Reml);
                    this.executionManager = new ExerciseExecutionManager(this.relManager.ParseRGML(), exercise.StressedJoints, database, settings, drawing, null);
                }

                GameManager.ExerciseIsActive = exerciseRuns = true;
                startTime = DateTime.Now.TimeOfDay.TotalSeconds;
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

            if (settings.GetValue<bool>("reading") && (!justStopped || audioSource.clip != exercise.AuditiveDescription))
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

            if (settings.GetValue<bool>("reading") && (!justStopped || exercise.AuditiveInformation))
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