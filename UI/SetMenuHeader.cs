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
                headerClip = Resources.Load(data.GetResource("auditiveTranslation", "mp3")) as AudioClip;
            }
            else
            {
                var data = DBManager.GetMenuHeader(this.name);
                headerText = data["name"].ToString();
                headerClip = data["clip"] as AudioClip;
            }

            textComponent.text = headerText;
        }
    }
}