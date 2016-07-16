namespace HSA.RehaGame.Manager.Audio
{
    using DB.Models;
    using Logging;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private static Logger<SoundManager> logger = new Logger<SoundManager>();

        private Queue<AudioClip> clips = new Queue<AudioClip>();
        private SettingsManager settingsManager;
        private SceneManager sceneManager;
        private AudioSource source;

        private SettingsKeyValue readingOn;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.settingsManager = this.GetComponentInParent<SettingsManager>();
            this.sceneManager = this.GetComponentInParent<SceneManager>();

            this.source = this.GetComponent<AudioSource>();
            this.readingOn = settingsManager.GetKeyValue("ingame", "reading");
        }

        void Update()
        {
            if (readingOn.GetValue<bool>() && source.isPlaying == false && clips.Count > 0)
            {
                source.clip = clips.Dequeue();
                source.Play();
            }
        }

        public void Stop()
        {
            source.Stop();
        }

        public void ReadingOnOff()
        {
            readingOn.SetValue<bool>(!readingOn.GetValue<bool>());
            sceneManager.ReloadSettingsMenu();
        }

        private void RemoveUnplayedClips()
        {
            clips.Clear();
        }

        public void Enqueue(AudioClip clip, bool clearUnplayed = false)
        {
            if (clips.Contains(clip) == false)
            {
                if (clearUnplayed && source.isPlaying && clips.Count > 0)
                    RemoveUnplayedClips();

                clips.Enqueue(clip);
            }
        }
        
        public bool isPlaying
        {
            get
            {
                return this.source.isPlaying;
            }
        }
    }
}
