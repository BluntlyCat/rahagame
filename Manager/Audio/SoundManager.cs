namespace HSA.RehaGame.Manager.Audio
{
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private Queue<AudioClip> clips = new Queue<AudioClip>();
        private SettingsManager settingsManager;
        private AudioSource source;

        void Start()
        {
            settingsManager = this.GetComponentInParent<SettingsManager>();
            source = this.GetComponent<AudioSource>();
        }

        void Update()
        {
            // ToDo Ton nur abspielen wenn Ton an
            if (source.isPlaying == false && clips.Count > 0)
            {
                source.clip = clips.Dequeue();
                source.Play();
            }
        }

        public void Enqueue(AudioClip clip)
        {
            if (clips.Contains(clip) == false)
                clips.Enqueue(clip);
        }
    }
}
