namespace HSA.RehaGame.DB
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    [RequireComponent (typeof(AudioSource))]

    public class GetVideo : MonoBehaviour
    {
        private MovieTexture movie;
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            var scene = SceneManager.GetActiveScene().name;
            var list = DBManager.Query("SELECT video from editor_exercise WHERE unityObjectName = '" + scene + "'");
            var file = list[0]["video"].ToString().Replace(".mp4", "").Replace("Assets/Resources/", "");
            movie = Resources.Load(file) as MovieTexture;

            GetComponent<RawImage>().texture = movie;
            audioSource = GetComponent<AudioSource>();

            audioSource.clip = movie.audioClip;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Play()
        {
            if(movie.isPlaying)
            {
                movie.Pause();
                audioSource.Pause();
            }
            else
            {
                movie.Play();
                audioSource.Play();
            }
        }
    }
}