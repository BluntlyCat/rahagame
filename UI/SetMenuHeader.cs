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
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            Text textComponent = this.GetComponent<Text>();
            AudioClip headerClip;
            string headerText;

            if (SceneManager.GetActiveScene().name == "newUser" && GameState.PatientName != "")
            {
                var data  = DBManager.GetTranslation("welcome");
                headerText = data["translation"].ToString() + ", " + GameState.PatientName; ;
                headerClip = data["clip"] as AudioClip;
            }
            else
            {
                var data = DBManager.GetMenuHeader(this.name);
                headerText = data["name"].ToString();
                headerClip = data["clip"] as AudioClip;
            }

            textComponent.text = headerText;
            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = headerClip;

            if(RGSettings.readingAloud)
                audioSource.Play();
        }
    }
}