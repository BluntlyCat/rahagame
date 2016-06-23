namespace HSA.RehaGame.UI.AuditiveExercise
{
    using Logging;
    using Manager.Audio;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public abstract class Pitchable : MonoBehaviour, IPitchable
    {
        protected static Logger<Pitchable> logger = new Logger<Pitchable>();

        private bool active;
        private AudioSource audioSource;

        void Start()
        {
            audioSource = this.GetComponent<AudioSource>();
        }

        protected void Pitch(float value = 0)
        {
            this.audioSource.pitch = value;

            if (this.audioSource.isPlaying == false)
                this.audioSource.Play();
        }

        public bool Active
        {
            get
            {
                return this.active;
            }

            set
            {
                this.active = value;
            }
        }

        public abstract void Replay(params object[] args);
    }
}
