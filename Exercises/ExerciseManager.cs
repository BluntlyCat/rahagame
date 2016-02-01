namespace HSA.RehaGame.Exercises
{
    using UI.VisualExercise;
    using InGame;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]
    public class ExerciseManager : MonoBehaviour
    {
        public GameObject DrawingPrefab;

        private Exercise exercise;
        private MovieTexture movieTexture;
        private AudioSource audioSource;
        private Vector2 scrollPosition;
        private Drawing drawing;

        // Use this for initialization
        void Start()
        {
            drawing = DrawingPrefab.GetComponent<Drawing>();

            if (GameState.ActiveExercise != null)
            {
                exercise = GameState.ActiveExercise;
            }
            else
            {
                // ToDo Set exercise from gamestate
                //GameState.ActivePatient = GameState.ActivePatient == null ? new Patient("Michael").Select() as Patient : GameState.ActivePatient;
                //GameState.ActiveExercise = GameState.ActiveExercise == null ? new Exercise("exercise1", GameState.ActivePatient).Select() as Exercise : GameState.ActiveExercise;
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

        void OnGUI()
        {
            /*scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(400), GUILayout.Height(240));
            GUILayout.Label(exercise.Description);
            if (GUILayout.Button("Clear"))
                isReading = false;

            GUILayout.EndScrollView();*/
        }

        public void StartExercise()
        {
            exercise.StartDoingExercise(drawing);
        }

        public void StopExercise()
        {
            exercise.StopDoingExercise();
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