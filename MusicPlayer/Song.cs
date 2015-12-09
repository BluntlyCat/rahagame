namespace HSA.RehaGame.MusicPlayer
{
    using System;
    using System.IO;
    using UnityEngine;

    public class Song
    {
        private Song prev_property;
        private Song next_property;

        private AudioClip clip_property;

        private bool enabled_property = true;
        private bool removable_property = true;

        public Song(AudioClip clip, Song prev, Song next, bool removable)
        {
            this.clip_property = clip;

            this.prev_property = prev;
            this.next_property = next;

            this.removable_property = removable;
        }

        public void Enable()
        {
            this.enabled_property = true;
        }

        public void Disable()
        {
            this.enabled_property = false;
        }

        public AudioClip clip
        {
            get
            {
                return this.clip_property;
            }
        }

        public Song prev
        {
            get
            {
                return this.prev_property;
            }
            set
            {
                this.prev_property = value;
            }
        }

        public Song next
        {
            get
            {
                return this.next_property;
            }
            set
            {
                this.next_property = value;
            }
        }

        public bool enabled
        {
            get
            {
                return this.enabled_property;
            }
        }

        public bool removable
        {
            get
            {
                return this.removable_property;
            }
        }
    }
}