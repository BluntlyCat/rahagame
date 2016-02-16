namespace HSA.RehaGame.DB.Models
{
    using UnityEngine;

    public class ValueTranslation : UnityModel
    {
        private string translation;

        private AudioClip auditiveTranslation;
        
        public ValueTranslation(string unityObjectName) : base (unityObjectName) {}

        [TranslationColumn]
        public string Translation
        {
            get
            {
                return translation;
            }

            set
            {
                this.translation = value;
            }
        }

        [TranslationColumn]
        [Resource]
        public AudioClip AuditiveTranslation
        {
            get
            {
                return auditiveTranslation;
            }

            set
            {
                this.auditiveTranslation = value;
            }
        }
    }
}
