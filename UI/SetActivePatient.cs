namespace HSA.RehaGame.UI
{
    using DB.Models;
    using Manager;
    using UnityEngine;

    public class SetActivePatient : MonoBehaviour
    {
        private GameObject gameManager;

        private SceneManager sceneManager;
        private PatientManager patientManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            patientManager = gameManager.GetComponent<PatientManager>();
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        public void SetPatient()
        {
            patientManager.ActivePatient = Model.GetModel<Patient>(this.name);
            sceneManager.LoadMainMenu();
        }
    }
}