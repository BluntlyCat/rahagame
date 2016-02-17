namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Menu : UnityModel
    {
        private string name;
        private AudioClip auditiveName;
        private Music music;
        private Dictionary<string, MenuEntry> entries;

        public Menu (string unityObjectName) : base (unityObjectName) { }

        [TranslationColumn]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.name = value;
            }
        }

        [TranslationColumn]
        [Resource]
        public AudioClip AuditiveName
        {
            get
            {
                return auditiveName;
            }

            set
            {
                this.auditiveName = value;
            }
        }

        [ForeignKey("music", "music_id", false)]
        public Music Music
        {
            get
            {
                return this.music;
            }

            set
            {
                this.music = value;
            }
        }

        [ManyToManyRelation(
            "unityObjectName",
            "menu",
            "menu_id",
            "menu_menuEntries",
            "menuentry_id",
            "menuentry",
            "unityObjectName"
        )]
        public Dictionary<string, MenuEntry> Entries
        {
            get
            {
                return entries;
            }

            set
            {
                this.entries = value;
            }
        }
    }
}
