namespace HSA.RehaGame.MusicPlayer
{
    using DB.Models;
    using Logging;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        public GameObject settingsPrefab;
        private Logger<MusicPlayer> logger = new Logger<MusicPlayer>();

        private Settings settings;
        private AudioSource audioSource;
        private string currentScene;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.settings = settingsPrefab.GetComponent<Settings>();
            this.audioSource = this.GetComponent<AudioSource>();
        }

        public void AddSong(string scene, string file)
        {
            
        }

        public void Play()
        {
            if (settings.GetValue<bool>("music"))
            {
                this.audioSource.Stop();
                this.audioSource.Play();
            }
        }

        public void Pause()
        {
            if(this.audioSource.isPlaying)
                this.audioSource.Pause();
            else
                this.audioSource.UnPause();
        }

        public void Stop()
        {
            this.audioSource.Stop();
        }

        public void Next()
        {
            this.Play();
        }

        public void Prev()
        {
            this.Play();
        }

        public bool isPlaying
        {
            get
            {
                return this.audioSource.isPlaying;
            }
        }

        public string playlist
        {
            set
            {
            }
        }
    }
}
