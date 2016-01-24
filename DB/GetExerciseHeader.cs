namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class GetExerciseHeader : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            var scene = SceneManager.GetActiveScene().name;
            var list = DBManager.Query("SELECT name from editor_exercise WHERE unityObjectName = '" + scene + "'");
            var text = this.GetComponent<Text>();
            text.text = list[0]["name"].ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}