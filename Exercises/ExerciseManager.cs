namespace HSA.RehaGame.Exercises
{
    using UnityEngine;
    using InGame;
    using UnityEngine.UI;
    using User;

    [RequireComponent(typeof(AudioSource))]

    public class ExerciseManager : MonoBehaviour
    {
        private Exercise exercise;
        private MovieTexture movieTexture;
        private AudioSource audioSource;
        private bool isReading = false;
        private Vector2 scrollPosition;

        // Use this for initialization
        void Start()
        {
            if (GameState.ActiveExercise != null)
            {
                exercise = GameState.ActiveExercise;
            }
            else
            {
                exercise = new Exercise("exercise1", new Patient("michael").Select() as Patient).Select() as Exercise;
                GameState.ActiveExercise = exercise;
            }

            movieTexture = exercise.Video;
            movieTexture.loop = true;

            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = exercise.AuditiveDescription;

            GameObject.Find("exerciseHeader").GetComponent<Text>().text = exercise.Name;

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

        void Update()
        {
            if(isReading && !audioSource.isPlaying)
            {
                audioSource.clip = exercise.AuditiveInformation;
                audioSource.Play();
                isReading = false;
            }
        }

        // Update is called once per frame
        public void PlayVideo()
        {
            movieTexture.Play();
        }

        public void ReadDescription()
        {
            if (RGSettings.reading)
            {
                audioSource.Play();
                isReading = true;
            }
        }
    }
}