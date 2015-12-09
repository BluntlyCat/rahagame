namespace HSA.RehaGame.MusicPlayer
{
    using System;
    using UnityEngine;

    public class Playlist
    {
        private Song first = null;
        private Song last = null;
        private Song current_property = null;

        private void ConnectSong(Song song)
        {
            if (first == null)
            {
                first = song;

                first.next = first;
                first.prev = first;
            }
            else
            {
                last.next = song;
                first.prev = song;
            }

            last = song;
        }

        public Playlist(string scene)
        {
            var clips = Resources.LoadAll<AudioClip>("Music/" + scene);

            foreach (var clip in clips)
            {
                this.AddSong(clip, false);
            }

            this.current_property = first;
        }

        public void AddSong(AudioClip clip, bool removable)
        {
            var song = new Song(clip, last, first, removable);
            this.ConnectSong(song);
        }

        public void RemoveSong(Song song)
        {
            if (song.removable)
            {
                var tmp = first;

                while (!tmp.Equals(song))
                    tmp = tmp.next;

                tmp.prev.next = tmp.next;
                tmp.next.prev = tmp.prev;

                tmp = null;
            }
        }

        public void Next()
        {
            this.current_property = this.current_property.next;
        }

        public void Prev()
        {
            this.current_property = this.current_property.prev;
        }

        public Song current
        {
            get
            {
                return this.current_property;
            }
            set
            {
                this.current_property = value;
            }
        }
    }
}