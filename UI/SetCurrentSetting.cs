namespace HSA.RehaGame.UI
{
    using DB;
    using InGame;
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
            var entry = database.Select("editor_menuentry", this.name);
            var value = database.Select("editor_valuetranslation", GetValue());

            text.text = string.Format("{0}: {1}", entry.Column("entry").GetValue<string>(), value.Column("translation").GetValue<string>());
            audioSource = this.GetComponent<AudioSource>();
            settingName = Resources.Load<AudioClip>(entry.Column("auditiveEntry").GetValue<string>());
            auditiveValue = Resources.Load<AudioClip>(value.Column("auditiveTranslation").GetValue<string>());
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
            return settings.GetByPropertyName(this, this.name);
        }

        public void Reading()
        {
            if (settings.reading)
            {
                var value = database.Select("editor_valuetranslation", GetValue());
                auditiveValue = Resources.Load<AudioClip>(value.Column("auditiveTranslation").GetValue<string>());

                audioSource.clip = settingName;
                audioSource.Play();
                isReading = true;
            }
        }
    }
}