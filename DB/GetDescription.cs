namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    [RequireComponent(typeof(AudioSource))]

    public class GetDescription : MonoBehaviour
    {
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            var scene = SceneManager.GetActiveScene().name;
            var desc = DBManager.Query("description", "editor_exercise", scene);
            var audioDesc = DBManager.Query("auditiveDescription", "editor_exercise", scene);

            var text = this.GetComponent<Text>();

            text.text = desc;

            var file = audioDesc.Replace(".mp3", "").Replace("Assets/Resources/", "");
            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = Resources.Load(file) as AudioClip;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Play()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Play();
            }
        }
    }
}