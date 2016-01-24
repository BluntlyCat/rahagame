namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;

    public class GetMenuEntry : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            var text = this.GetComponentInChildren<Text>();
            text.text = DBManager.Query("entry", "editor_menuentry", this.name);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}