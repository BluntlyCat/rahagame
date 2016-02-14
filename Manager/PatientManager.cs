namespace HSA.RehaGame.Manager
{
    using System.Collections.Generic;
    using DB.Models;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]
    public class PatientManager : BaseModelManager<Patient>
    {
        public GameObject gameManager;

        private SettingsManager settingsManager;
        private SceneManager sceneManager;

        private Patient patient;
        private bool queryDone;
        private bool isReading = false;

        private InputField nameInput;
        private InputField ageInput;
        private Dropdown sexInput;
        private Button saveButton;

        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            settingsManager = gameManager.GetComponent<SettingsManager>();
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        private void ActivateJoints()
        {
            var jointPanel = GameObject.Find("jointList");

            foreach (Transform joint in jointPanel.transform)
            {
                joint.gameObject.SetActive(true);
            }
        }

        public void Save()
        {
            if (patient != null)
            {
                patient.Age = int.Parse(ageInput.text);
                patient.Sex = (Sex)sexInput.value;

                patient.Save();
            }
            else
            {
                patient = new Patient(nameInput.text, int.Parse(ageInput.text), (Sex)sexInput.value);
                patient.Save();
            }

            if (queryDone)
            {
                GameManager.ActivePatient = patient;

                ActivateJoints();
            }
        }

        public void Delete()
        {
            if (patient != null)
            {
                // ToDo patient.Delete();
                queryDone = false;
            }
        }
    }
}