namespace HSA.RehaGame.MusicPlayer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class DefaultPlaylists : Dictionary<string, Playlist>
    {
        public DefaultPlaylists()
        {
            this.NewPlaylist("menu");
            this.NewPlaylist("RGSandbox");
        }

        private void NewPlaylist(string scene)
        {
            this.Add(scene, new Playlist(scene));
        }
    }
}
