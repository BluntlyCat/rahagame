namespace HSA.RehaGame.DB.Models
{
    using UnityEngine;

    public class Equipment : UnityModel
    {
        private string name;
        private string description;
        private AudioClip auditiveDescription;
        private Texture2D image;

        public Equipment(string unityObjectName) : base (unityObjectName)
        {

        }

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
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.description = value;
            }
        }

        [TranslationColumn]
        [Resource]
        public AudioClip AuditiveDescription
        {
            get
            {
                return auditiveDescription;
            }

            set
            {
                this.auditiveDescription = value;
            }
        }

        [TableColumn]
        [Resource]
        public Texture2D Image
        {
            get
            {
                return image;
            }

            set
            {
                this.image = value;
            }
        }
    }
}
