namespace HSA.RehaGame.Manager
{
    using Audio;
    using DB = DB.Models;
    using Exercises.FulFillables;
    using Input.Kinect;
    using System.Collections.Generic;
    using UI;
    using UI.Feedback;
    using UnityEngine;
    using UnityEngine.UI;
    using Windows.Kinect;
    using UI.Statistic.Views;
    public class ExerciseManager : MonoBehaviour
    {
        public GameObject gameManager;
        public GameObject drawingPrefab;
        public GameObject waitPanel;
        public GameObject menuTitle;
        public GameObject menuContainer;

        private IDictionary<object, DB.Exercise> models;

        private MovieTexture movieTexture;

        private SettingsManager settingsManager;
        private WriteStatisticManager statisticManager;
        private SoundManager soundManager;
        private SceneManager sceneManager;
        private BodyManager bodyManager;

        private Feedback feedback;

        private SwapCanvas swapCanvas;

        private Body body;
        private PatientManager patientManager;
        private REMLManager relManager;

        private Exercise exercise;

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
            statisticManager = this.GetComponentInChildren<WriteStatisticManager>();

            bodyManager.BodyDetected += BodyManager_BodyDetected;
            bodyManager.BodyLost += BodyManager_BodyLost;

            waitPanel.GetComponentInChildren<Text>().text = DB.Model.GetModel<DB.ValueTranslation>("noUser").Translation;

            patientManager.SetStressedJoints(GameManager.ActiveExercise);

            movieTexture = GameManager.ActiveExercise.Video;
            movieTexture.loop = true;

            menuTitle.GetComponentInChildren<Text>().text = GameManager.ActiveExercise.Name;

            GameObject.Find("Video").GetComponent<RawImage>().texture = movieTexture;
            GameObject.Find("Description").GetComponentInChildren<Text>().text = GameManager.ActiveExercise.Description;
            GameObject.Find("Information").GetComponent<Text>().text = GameManager.ActiveExercise.Information;

        }

        private void BodyManager_BodyLost()
        {
            if (this.body != null)
            {
                this.body = null;

                if (exerciseRuns)
                {
                    exercise.Canceled();
                    LoadStatistic();
                }

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
            if (body != null && GameObject.Find(GameManager.ActivePatient.Name) != null)
            {
                if (exerciseRuns)
                {
                    isFullfilled = exercise.IsFulfilled(body);

                    if (isFullfilled)
                    {
                        exercise.Fulfilled();
                        LoadStatistic();
                    }
                }
            }
        }

        private void LoadStatistic()
        {
            exerciseRuns = false;

            GameManager.StatisticViewData.StatisticViewType = StatisticViewTypes.byCurrentExercise;
            GameManager.StatisticViewData.Data = GameManager.ActivePatient.CurrentStatistic;
            BodySourceManager.ShutdownKinect();

            statisticManager.SaveStatistic();
            sceneManager.LoadStatisticMenu();
        }

        public void StartExercise()
        {
            if (hasUser)
            {
                swapCanvas.SwapVisibility();
                this.StopMedia();

                if (relManager == null)
                {
                    this.relManager = new REMLManager(GameManager.ActivePatient, settingsManager, statisticManager, feedback, GameManager.ActiveExercise.Reml);
                    this.exercise = this.relManager.ParseRGML();
                }

                GameManager.ExerciseIsActive = exerciseRuns = true;
                exercise.Unfulfilled();
            }
        }

        public void StopExercise()
        {
            GameManager.ExerciseIsActive = exerciseRuns = false;
            exercise.Fulfilled();
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

            if (settingsManager.GetValue<bool>("ingame", "reading") && (!justStopped || GameManager.ActiveExercise.AuditiveDescription))
            {
                soundManager.Enqueue(GameManager.ActiveExercise.AuditiveDescription);
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

            if (settingsManager.GetValue<bool>("ingame", "reading") && (!justStopped || GameManager.ActiveExercise.AuditiveInformation))
            {
                soundManager.Enqueue(GameManager.ActiveExercise.AuditiveInformation);
            }
        }
    }
}