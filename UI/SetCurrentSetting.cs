namespace HSA.RehaGame.UI
{
    using DB;
    using InGame;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]

    public class SetCurrentSetting : MonoBehaviour
    {
        private AudioSource audioSource;
        private AudioClip settingName;
        private AudioClip auditiveValue;
        private bool isReading = false;

        // Use this for initialization
        void Start()
        {
            var text = this.GetComponentInChildren<Text>();
            var name = DBManager.Query("editor_menuentry", string.Format("SELECT * FROM editor_menuentry WHERE unityObjectName = '{0}'", this.name));
            var trans = GetValue();

            text.text = string.Format("{0}: {1}", name.GetValueFromLanguage("entry"), trans.GetValue("translation"));
            audioSource = this.GetComponent<AudioSource>();
            settingName = Resources.Load(name.GetResource("auditiveEntry", "mp3")) as AudioClip;
            auditiveValue = Resources.Load(trans.GetResource("auditiveTranslation", "mp3")) as AudioClip;
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

        private DBTable GetValue()
        {
            var value = RGSettings.GetByPropertyName(this, this.name);
            return DBManager.GetTranslation(value);
        }

        public void Reading()
        {
            if (RGSettings.reading)
            {
                var trans = GetValue();
                auditiveValue = Resources.Load(trans.GetResource("auditiveTranslation", "mp3")) as AudioClip;

                audioSource.clip = settingName;
                audioSource.Play();
                isReading = true;
            }
        }
    }
}