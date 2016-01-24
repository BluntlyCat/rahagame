namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System.Linq;
    using UnityEngine.UI;

    public class GetInputField : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            var text = (from t in this.GetComponentsInChildren<Text>() where t.name == "Placeholder" select t).First();
            text.text = DBManager.Query("entry", "editor_menuentry", this.name);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}