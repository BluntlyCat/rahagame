namespace HSA.RehaGame.UI
{
    using DB.Models;
    using Manager;
    using UnityEngine;

    public class SetActivePatient : MonoBehaviour
    {
        public GameObject gameManager;

        private SceneManager sceneManager;
        private PatientManager patientManager;

        void Start()
        {
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        public void SetPatient()
        {
            patientManager.ActivePatient = Model.GetModel<Patient>(this.name);
            sceneManager.LoadMainMenu();
        }
    }
}