namespace HSA.RehaGame.UI
{
    using DB;
    using InGame;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class SetMenuHeader : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            Text textComponent = this.GetComponent<Text>();
            AudioClip headerClip;
            string headerText;

            if (SceneManager.GetActiveScene().name == "newUser" && GameState.ActivePatient != null)
            {
                var data  = DBManager.GetTranslation("welcome");
                headerText = string.Format("{0}, {1}", data.GetValueFromLanguage("translation"), GameState.ActivePatient.Name);
                headerClip = data.GetResource<AudioClip>("auditiveTranslation", "mp3");
            }
            else
            {
                var data = DBManager.GetMenuHeader(this.name);
                headerText = data.GetValueFromLanguage("name");
                headerClip = data.GetResource<AudioClip>("auditiveName", "mp3");
            }

            textComponent.text = headerText;
        }
    }
}