namespace HSA.RehaGame.InGame
{
    using DB;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]

    public class GetCurrentSetting : MonoBehaviour
    {
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            var text = this.GetComponentInChildren<Text>();
            var name = DBManager.Query("entry", "editor_menuentry", this.name);
            var value = RGSettings.GetByPropertyName(this, this.name);
            var trans = DBManager.GetTranslation(value);

            text.text = name + ": " + trans["translation"].ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}