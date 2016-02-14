namespace HSA.RehaGame.Manager
{
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private Queue<AudioClip> clips = new Queue<AudioClip>();
        private AudioSource source;

        void Start()
        {
            source = this.GetComponent<AudioSource>();
        }

        void Update()
        {
            if (source.isPlaying == false && clips.Count > 0)
            {
                source.clip = clips.Dequeue();
                source.Play();
            }
        }

        public void Enqueue(AudioClip clip)
        {
            clips.Enqueue(clip);
        }
    }
}
