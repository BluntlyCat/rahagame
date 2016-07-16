namespace HSA.RehaGame.UI
{
    using DB.Models;
    using Manager;
    using UnityEngine;

    public class SetActivePatient : MonoBehaviour
    {
        private GameObject gameManager;

        private SceneManager sceneManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        public void SetPatient()
        {
            GameManager.ActivePatient = Model.GetModel<Patient>(this.name);
            sceneManager.LoadTitleMenu();
        }
    }
}