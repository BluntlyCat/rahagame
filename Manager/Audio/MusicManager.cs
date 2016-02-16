namespace HSA.RehaGame.Manager.Audio
{
    using System.Collections.Generic;
    using DB.Models;
    using Logging;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        private Logger<MusicManager> logger = new Logger<MusicManager>();

        private SettingsManager settingsManager;
        private AudioSource audioSource;

        private LinkedList<Music> playlist;
        private LinkedListNode<Music> current;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.settingsManager = this.GetComponentInParent<SettingsManager>();
            this.audioSource = this.GetComponent<AudioSource>();
        }

        void Update()
        {
            // ToDo Musik nur abspielen wenn Musik an
            if(audioSource.isPlaying == false)
            {
                current = current.Next;
                this.Play();
            }
        }

        public void Play()
        {
            if (this.audioSource.clip == null)
                current = playlist.First;

            this.audioSource.clip = current.Value.Title;
            this.audioSource.Play();
        }

        public void Pause()
        {
            if (this.audioSource.isPlaying)
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
            current = current.Next;
            this.audioSource.clip = current.Value.Title;
        }

        public void Prev()
        {
            current = current.Previous;
            this.audioSource.clip = current.Value.Title;
        }

        public bool isPlaying
        {
            get
            {
                return this.audioSource.isPlaying;
            }
        }
    }
}
