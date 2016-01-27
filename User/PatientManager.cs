namespace HSA.RehaGame.User
{
    using UnityEngine;
    using InGame;
    using UnityEngine.UI;

    public class PatientManager : MonoBehaviour
    {
        private Patient patient;
        private bool update;

        private InputField nameInput;
        private InputField ageInput;
        private InputField sexInput;

        // Use this for initialization
        void Start()
        {
            var inputFields = GameObject.Find("Image").GetComponentsInChildren<InputField>();

            nameInput = inputFields[0];
            ageInput = inputFields[1];
            sexInput = inputFields[2];

            if (GameState.PatientName != null)
            {
                patient = GameState.ActivePatient;

                nameInput.text = patient.Name;
                ageInput.text = patient.Age;
                sexInput.text = patient.Sex;

                ActivateJoints();
            }
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
                update = true;

                patient.Age = ageInput.text;
                patient.Sex = sexInput.text;
            }
            else
            {
                update = false;
                patient = new Patient(nameInput.text, ageInput.text, sexInput.text);
            }

            if (patient.Save(update))
            {
                ActivateJoints();
            }
        }

        public void Delete()
        {
            if (patient != null)
            {
                patient.Delete();
                update = false;
            }
        }
    }
}