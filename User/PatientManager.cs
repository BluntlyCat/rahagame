namespace HSA.RehaGame.User
{
    using UnityEngine;
    using InGame;
    using UnityEngine.UI;
    using DB;

    public class PatientManager : MonoBehaviour
    {
        private Patient patient;
        private bool queryDone;

        private InputField nameInput;
        private InputField ageInput;
        private Dropdown sexInput;

        // Use this for initialization
        void Start()
        {
            var name = DBManager.Query("editor_valuetranslation", "SELECT * FROM editor_valuetranslation WHERE unityObjectName = 'name'");
            var age = DBManager.Query("editor_valuetranslation", "SELECT * FROM editor_valuetranslation WHERE unityObjectName = 'age'");
            var sex = DBManager.Query("editor_valuetranslation", "SELECT * FROM editor_valuetranslation WHERE unityObjectName = 'sex'");

            nameInput = GameObject.Find("name").GetComponent<InputField>();
            ageInput = GameObject.Find("age").GetComponent<InputField>();
            sexInput = GameObject.Find("sex").GetComponent<Dropdown>();

            GameObject.Find("nameLabel").GetComponent<Text>().text = string.Format("{0}: ", name.GetValueFromLanguage("translation"));
            GameObject.Find("ageLabel").GetComponent<Text>().text = string.Format("{0}: ", age.GetValueFromLanguage("translation"));
            GameObject.Find("sexLabel").GetComponent<Text>().text = string.Format("{0}: ", sex.GetValueFromLanguage("translation"));



            if (GameState.ActivePatient != null)
            {
                patient = GameState.ActivePatient;

                nameInput.text = patient.Name;
                ageInput.text = patient.Age.ToString();
                sexInput.value = (int)patient.Sex;

                sexInput.RefreshShownValue();
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
                patient.Age = int.Parse(ageInput.text);
                patient.Sex = (Gender)sexInput.value;

                queryDone = patient.Update();
            }
            else
            {
                patient = new Patient(nameInput.text, int.Parse(ageInput.text), (Gender)sexInput.value);
                queryDone = patient.Insert() != null;
            }

            if (queryDone)
            {
                GameState.ActivePatient = patient;

                ActivateJoints();
            }
        }

        public void Delete()
        {
            if (patient != null)
            {
                patient.Delete();
                queryDone = false;
            }
        }
    }
}