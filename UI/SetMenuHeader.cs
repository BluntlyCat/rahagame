namespace HSA.RehaGame.UI
{
    using DB;
    using InGame;
    using UnityEngine;
    using UnityEngine.SceneManagement;
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
            var data = gameManager.GetComponent<Database>().GetMenuHeader(this.name);

            audioSource = this.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            headerText = data.GetValueFromLanguage("name");
            headerClip = data.GetResource<AudioClip>("auditiveName", "mp3");
            audioSource.clip = headerClip;

            if (SceneManager.GetActiveScene().name == "NewUser" && GameState.ActivePatient != null)
            {
                audioSource.Play();   
            }
            else if (SceneManager.GetActiveScene().name != "NewUser")
            {
                audioSource.Play();
            }

            textComponent.text = headerText;
        }
    }
}