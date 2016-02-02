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

    [RequireComponent(typeof(AudioSource))]
    public class ExerciseManager : MonoBehaviour
    {
        public GameObject DrawingPrefab;
        public GameObject MenuPrefab;
        public GameObject waitPanel;
        public BodyManager bodyManager;

        private MovieTexture movieTexture;
        private AudioSource audioSource;
        private Drawing drawing;

        private SwapCanvas swapCanvas;

        private Exercise exercise;
        private Patient patient;
        private RELManager relManager;
        private ExerciseExecutionManager executionManager;

        private bool hasUser;
        private bool exerciseRuns;
        private bool isFullfilled;

        // Use this for initialization
        void Start()
        {
            drawing = DrawingPrefab.GetComponent<Drawing>();
            bodyManager = bodyManager.GetComponent<BodyManager>();

            waitPanel.GetComponentInChildren<Text>().text = DBManager.GetTranslation("noUser").GetValueFromLanguage("translation");
            swapCanvas = MenuPrefab.GetComponent<SwapCanvas>();

            if (GameState.ActiveExercise != null)
            {
                exercise = GameState.ActiveExercise;
            }
            else
            {
                // ToDo Set exercise from gamestate
                GameState.ActivePatient = GameState.ActivePatient == null ? new Patient("Michael").Select() as Patient : GameState.ActivePatient;
                GameState.ActiveExercise = GameState.ActiveExercise == null ? new Exercise("exercise1").Select() as Exercise : GameState.ActiveExercise;

                patient = GameState.ActivePatient;
                exercise = GameState.ActiveExercise;
            }

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
            if (GameObject.Find(GameState.ActivePatient.Name) != null)
            {
                GameState.HasKinectUser = hasUser = true;
                waitPanel.SetActive(false);

                if (exerciseRuns)
                {
                    isFullfilled = executionManager.IsFulfilled(bodyManager.GetBody());

                    if (isFullfilled)
                    {
                        exerciseRuns = false;
                        BodySourceManager.ShutdownKinect();
                        LoadScene.LoadStatistics();
                    }
                    else
                    {
                        drawing.ShowInformation(executionManager.Information());
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
                    this.relManager = new RELManager(patient, drawing, exercise.REL);
                    this.executionManager = new ExerciseExecutionManager(this.relManager.GetSteps(), exercise.StressedJoints, null);
                }

                GameState.ExerciseIsActive = exerciseRuns = true;
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
    }
}