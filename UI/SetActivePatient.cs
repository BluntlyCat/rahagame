namespace HSA.RehaGame.UI
{
    using DB;
    using DB.Models;
    using Manager;
    using UnityEngine;

    public class SetActivePatient : MonoBehaviour
    {
        public GameObject sceneManagerPrefab;
        public GameObject databaseManagerPrefab;

        private SceneManager sceneManager;
        private Database dbManager;

        void Start()
        {
            sceneManager = sceneManagerPrefab.GetComponent<SceneManager>();
            dbManager = databaseManagerPrefab.GetComponent<Database>();
        }

        public void SetPatient()
        {
            GameManager.ActivePatient = Model.GetModel<Patient>(this.name);
            sceneManager.LoadMainMenu();
        }
    }
}