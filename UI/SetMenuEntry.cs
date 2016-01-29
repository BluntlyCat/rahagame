namespace HSA.RehaGame.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using DB;
    using InGame;

    [RequireComponent(typeof(AudioSource))]

    public class SetMenuEntry : MonoBehaviour
    {
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            var text = this.GetComponentInChildren<Text>();
            var table = DBManager.Query("editor_menuentry", string.Format("SELECT * FROM editor_menuentry WHERE unityObjectName = '{0}'", this.name));

            audioSource = this.GetComponent<AudioSource>();
            text.text = table.GetValueFromLanguage("entry");
            audioSource.clip = Resources.Load(table.GetResource("auditiveEntry", "mp3")) as AudioClip;
        }

        public void Reading()
        {
            if (RGSettings.reading)
                audioSource.Play();
        }
    }
}