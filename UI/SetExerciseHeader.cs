namespace HSA.RehaGame.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using DB;

    public class SetExerciseHeader : MonoBehaviour
    {
        public GameObject dbManager;
        // Use this for initialization
        void Start()
        {
            var scene = SceneManager.GetActiveScene().name;
            var list = dbManager.GetComponent<Database>().Query("editor_exercise", "SELECT name FROM editor_exercise WHERE unityObjectName = '" + scene + "'");
            var text = this.GetComponent<Text>();
            text.text = list.GetValue("name");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}