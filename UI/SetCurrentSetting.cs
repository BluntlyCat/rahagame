namespace HSA.RehaGame.UI
{
    using DB;
    using DB.Models;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Database))]
    [RequireComponent(typeof(Settings))]
    [RequireComponent(typeof(AudioSource))]
    public class SetCurrentSetting : MonoBehaviour
    {
        private Database database;
        private Settings settings;
        private AudioSource audioSource;
        private AudioClip settingName;
        private AudioClip auditiveValue;
        private bool isReading = false;

        // Use this for initialization
        void Start()
        {
            database = this.GetComponent<Database>();
            settings = this.GetComponent<Settings>();

            var text = this.GetComponentInChildren<Text>();
            var entry = Model.GetModel<MenuEntry>(this.name);
            var value = Model.GetModel<ValueTranslation>(GetValue());

            text.text = string.Format("{0}: {1}", entry.Entry, value.Translation);
            audioSource = this.GetComponent<AudioSource>();
            settingName = entry.AuditiveEntry;
            auditiveValue = value.AuditiveTranslation;
        }

        void Update()
        {
            if (isReading && !audioSource.isPlaying)
            {
                audioSource.clip = auditiveValue;
                audioSource.Play();
                isReading = false;
            }
        }

        private string GetValue()
        {
            return settings.GetValue<string>(this.name);
        }

        public void Reading()
        {
            if (settings.GetValue<bool>("reading"))
            {
                var value = Model.GetModel<ValueTranslation>(GetValue());
                auditiveValue = value.AuditiveTranslation;

                audioSource.clip = settingName;
                audioSource.Play();
                isReading = true;
            }
        }
    }
}