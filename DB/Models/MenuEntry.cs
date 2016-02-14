namespace HSA.RehaGame.DB.Models
{
    using UnityEngine;

    public class MenuEntry : UnityModel
    {
        private string entry;
        private AudioClip auditiveEntry;
        private string placeholder;

        public MenuEntry(string unityObjectName) : base(unityObjectName) { }

        [TranslationColumn]
        public string Entry
        {
            get
            {
                return entry;
            }

            set
            {
                this.entry = value;
            }
        }

        [TranslationColumn]
        [ResourceColumn]
        public AudioClip AuditiveEntry
        {
            get
            {
                return auditiveEntry;
            }

            set
            {
                this.auditiveEntry = value;
            }
        }

        [TranslationColumn]
        public string Placeholder
        {
            get
            {
                return placeholder;
            }

            set
            {
                this.placeholder = value;
            }
        }
    }
}
