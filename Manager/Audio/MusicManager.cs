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

        private LinkedList<Music> playlist = new LinkedList<Music>();
        private LinkedListNode<Music> current;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.settingsManager = this.GetComponentInParent<SettingsManager>();
            this.audioSource = this.GetComponent<AudioSource>();
        }

        void Update()
        {
            if (current != null)
            {
                // ToDo Musik nur abspielen wenn Musik an
                if (audioSource.isPlaying == false)
                {
                    if (current.Next != null)
                        current = current.Next;
                    else
                        current = playlist.First;

                    this.Play();
                }
            }
        }

        public void AddMusic(Music music)
        {
            if (playlist.First == null)
            {
                playlist.AddFirst(music);
                current = playlist.First;
            }
            else
            {
                playlist.AddLast(music);
            }

            if (audioSource.isPlaying == false)
                Play();
        }

        public void Play()
        {
            if (this.playlist.First == null)
                return;

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
