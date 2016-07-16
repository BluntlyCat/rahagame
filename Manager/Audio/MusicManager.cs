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
        private SceneManager sceneManager;
        private AudioSource audioSource;

        private LinkedList<Music> playlist = new LinkedList<Music>();
        private LinkedListNode<Music> current;

        private SettingsKeyValue musicOn;

        void Awake()
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.settingsManager = this.GetComponentInParent<SettingsManager>();
            this.sceneManager = this.GetComponentInParent<SceneManager>();

            this.musicOn = settingsManager.GetKeyValue("ingame", "music");
            this.audioSource = this.GetComponent<AudioSource>();
        }

        void Update()
        {
            if (current != null)
            {
                if (musicOn.GetValue<bool>() && audioSource.isPlaying == false)
                {
                    if (current.Next != null)
                        current = current.Next;
                    else
                        current = playlist.First;

                    this.Play();
                }
            }
        }

        public void MusicOnOff()
        {
            this.musicOn.SetValue<bool>(!musicOn.GetValue<bool>());
            this.sceneManager.ReloadSettingsMenu();
        }

        public void AddMusic(Music music)
        {
            if (music == null)
                return;

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

            if (musicOn.GetValue<bool>())
            {
                this.audioSource.clip = current.Value.Title;
                this.audioSource.Play();
            }
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
