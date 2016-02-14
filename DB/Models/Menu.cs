namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Menu : UnityModel
    {
        private string name;
        private AudioClip auditiveName;
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
        [ResourceColumn]
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
