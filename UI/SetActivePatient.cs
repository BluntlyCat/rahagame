namespace HSA.RehaGame.UI
{
    using User;
    using Scene;
    using InGame;
    using UnityEngine;
    using DB;

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
            if (dbManager.Exists("editor_patient", this.name))
            {
                GameState.ActivePatient = new Patient(this.name, sceneManager, dbManager).Select() as Patient;
                sceneManager.LoadMainMenu();
            }
        }
    }
}