namespace HSA.RehaGame.MusicPlayer
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;
    using Logging;
    using Settings;

    public class MusicPlayer
    {
        private Logger<MusicPlayer> logger = new Logger<MusicPlayer>();

        private AudioSource player;
        private DefaultPlaylists playlists = new DefaultPlaylists();
        private string currentScene;

        public MusicPlayer(AudioSource audioSource)
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.player = audioSource;
            this.currentScene = this.playlists.First().Key;
        }

        public void AddSong(string scene, string file)
        {
            
        }

        public void Play()
        {
            if (RGSettings.music)
            {
                this.player.Stop();
                this.player.clip = this.playlists[currentScene].current.clip;
                this.player.Play();
            }
        }

        public void Pause()
        {
            if(this.player.isPlaying)
                this.player.Pause();
            else
                this.player.UnPause();
        }

        public void Stop()
        {
            this.player.Stop();
        }

        public void Next()
        {
            this.playlists[currentScene].Next();
            this.Play();
        }

        public void Prev()
        {
            this.playlists[currentScene].Next();
            this.Play();
        }

        public bool isPlaying
        {
            get
            {
                return this.player.isPlaying;
            }
        }

        public string playlist
        {
            set
            {
                if (this.playlists.ContainsKey(value))
                    this.currentScene = value;
                else
                    logger.Debug("Scene has no Playlist, use default");
            }
        }
    }
}
