namespace HSA.RehaGame.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using DB;
    using DB.Models;

    public class SetExerciseHeader : MonoBehaviour
    {
        public GameObject dbManager;
        // Use this for initialization
        void Start()
        {
            var scene = SceneManager.GetActiveScene().name;
            var text = this.GetComponent<Text>();
            text.text = Model.GetModel<Exercise>(scene).Name;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}