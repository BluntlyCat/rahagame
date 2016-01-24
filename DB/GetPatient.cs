namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using UnityEngine.UI;
    using User;
    using Settings;

    [RequireComponent (typeof(AudioSource))]

    public class GetPatient : MonoBehaviour
    {
        private Patient patient;
        private bool update = false;
        private InputField[] fields;
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            Text header = GameObject.Find("newUser").GetComponent<Text>();
            fields = this.GetComponentsInChildren<InputField>();

            if (RGSettings.ActivePatient != null)
            {
                patient = RGSettings.ActivePatient;
                header.text = RGSettings.GetTranslation("welcome") + ", " + patient.Name;

                fields[0].text = patient.Name;
                fields[1].text = patient.Age;
                fields[2].text = patient.Sex;

                ActivateJoints();
            }
            else
            {
                var data = DBManager.GetMenuHeader(header.gameObject.name);

                header.text = data["name"].ToString();

                audioSource = this.GetComponent<AudioSource>();
                audioSource.clip = data["clip"] as AudioClip;

                if (RGSettings.readingAloud)
                    audioSource.Play();
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

                patient.Age = fields[1].text;
                patient.Sex = fields[2].text;
            }
            else
            {
                update = false;

                patient = new Patient(fields[0].text, fields[1].text, fields[2].text);
            }

            if(patient.Save(update))
            {
                ActivateJoints();
            }
        }

        public void Delete()
        {
            if (patient != null)
            {
                patient.Delete();
            }
        }
    }
}