namespace HSA.RehaGame.UI
{
    using Models = DB.Models;
    using Manager;
    using UnityEngine;
    using UE = UnityEngine.SceneManagement;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]
    public class SetMenuHeader : MonoBehaviour
    {
        public GameObject gameManager;
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            Text textComponent = this.GetComponent<Text>();
            AudioClip headerClip;
            string headerText;
            var menu = Models.Model.GetModel<Models.Menu>(this.name);

            audioSource = this.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            headerText = menu.Name;
            headerClip = menu.AuditiveName;
            audioSource.clip = headerClip;

            if (UE.SceneManager.GetActiveScene().name == "NewUser" && GameManager.ActivePatient != null)
            {
                audioSource.Play();   
            }
            else if (UE.SceneManager.GetActiveScene().name != "NewUser")
            {
                audioSource.Play();
            }

            textComponent.text = headerText;
        }
    }
}