namespace HSA.RehaGame.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using DB;
    using InGame;

    [RequireComponent(typeof(AudioSource))]

    public class SetMenuEntry : MonoBehaviour
    {
        public GameObject gameManager;

        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            var text = this.GetComponentInChildren<Text>();
            var table = gameManager.GetComponent<Database>().Query("editor_menuentry", string.Format("SELECT * FROM editor_menuentry WHERE unityObjectName = '{0}'", this.name));

            audioSource = this.GetComponent<AudioSource>();
            text.text = table.GetValueFromLanguage("entry");
            audioSource.clip = table.GetResource<AudioClip>("auditiveEntry", "mp3");
        }

        public void Reading()
        {
            if (gameManager.GetComponent<Settings>().reading)
                audioSource.Play();
        }
    }
}