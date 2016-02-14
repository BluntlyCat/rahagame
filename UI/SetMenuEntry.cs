namespace HSA.RehaGame.UI
{
    using DB.Models;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]

    public class SetMenuEntry : MonoBehaviour
    {
        public GameObject gameManager;

        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            var text = this.GetComponentInChildren<Text>();
            var entry = Model.GetModel<MenuEntry>(this.name);

            audioSource = this.GetComponent<AudioSource>();
            text.text = entry.Entry;
            audioSource.clip = entry.AuditiveEntry;
        }

        public void Reading()
        {
            if (gameManager.GetComponent<Settings>().GetValue<bool>("reading"))
                audioSource.Play();
        }
    }
}