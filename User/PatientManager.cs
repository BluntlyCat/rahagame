namespace HSA.RehaGame.User
{
    using UnityEngine;
    using InGame;
    using UnityEngine.UI;
    using DB;
    using UnityEngine.EventSystems;
    using System.Collections.Generic;

    [RequireComponent(typeof(AudioSource))]

    public class PatientManager : MonoBehaviour
    {
        private Patient patient;
        private bool queryDone;
        private bool isReading = false;

        private InputField nameInput;
        private InputField ageInput;
        private Dropdown sexInput;
        private Button saveButton;

        private AudioSource audioSource;

        private AudioClip nameClip;
        private AudioClip ageClip;
        private AudioClip sexClip;
        private AudioClip deleteClip;
        private AudioClip saveClip;
        private AudioClip maleClip;
        private AudioClip femaleClip;
        private List<string> options;

        // Use this for initialization
        void Start()
        {
            var name = DBManager.GetTranslation("name");
            var age = DBManager.GetTranslation("age");
            var sex = DBManager.GetTranslation("sex");
            var save = DBManager.GetTranslation("save");
            var delete = DBManager.GetTranslation("delete");

            var genders = DBManager.GetTranslation(Logics.OR, Orders.desc, "male", "female");

            options = new List<string>();

            audioSource = this.GetComponent<AudioSource>();

            nameInput = GameObject.Find("name").GetComponent<InputField>();
            ageInput = GameObject.Find("age").GetComponent<InputField>();
            sexInput = GameObject.Find("sex").GetComponent<Dropdown>();
            saveButton = GameObject.Find("savePatient").GetComponent<Button>();

            GameObject.Find("nameLabel").GetComponent<Text>().text = string.Format("{0}: ", name.GetValueFromLanguage("translation"));
            GameObject.Find("ageLabel").GetComponent<Text>().text = string.Format("{0}: ", age.GetValueFromLanguage("translation"));
            GameObject.Find("sexOuterLabel").GetComponent<Text>().text = string.Format("{0}: ", sex.GetValueFromLanguage("translation"));

            nameClip = name.GetResource<AudioClip>("auditiveTranslation", "mp3");
            ageClip = age.GetResource<AudioClip>("auditiveTranslation", "mp3");
            sexClip = sex.GetResource<AudioClip>("auditiveTranslation", "mp3");
            deleteClip = delete.GetResource<AudioClip>("auditiveTranslation", "mp3");
            saveClip = save.GetResource<AudioClip>("auditiveTranslation", "mp3");

            foreach (var gender in genders.Rows)
            {
                options.Add(gender.GetValueFromLanguage("translation"));

                if (gender.GetValue("unityObjectName") == "male")
                    maleClip = gender.GetResource<AudioClip>("auditiveTranslation", "mp3");
                else if (gender.GetValue("unityObjectName") == "female")
                    femaleClip = gender.GetResource<AudioClip>("auditiveTranslation", "mp3");
            }

            sexInput.AddOptions(options);

            if (GameState.ActivePatient != null)
            {
                patient = GameState.ActivePatient;

                nameInput.text = patient.Name;
                ageInput.text = patient.Age.ToString();
                sexInput.value = (int)patient.Sex;
                sexInput.GetComponentInChildren<Text>().text = options[(int)patient.Sex];

                sexInput.RefreshShownValue();
                ActivateJoints();

                GameObject.Find("EventSystem").GetComponent<EventSystem>().firstSelectedGameObject = GameObject.Find("saveUser");
            }
            else
                sexInput.GetComponentInChildren<Text>().text = options[0];
        }

        void Update()
        {
            isReading = audioSource.isPlaying;

            if (!isReading && (audioSource.clip == maleClip || audioSource.clip == femaleClip))
                saveButton.Select();
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

        public void ReadName()
        {
            if (RGSettings.reading && !isReading)
            {
                audioSource.clip = nameClip;

                isReading = true;
                audioSource.Play();
            }
        }
        public void ReadAge()
        {
            if (RGSettings.reading && !isReading)
            {
                audioSource.clip = ageClip;

                isReading = true;
                audioSource.Play();
            }
        }
        public void ReadSex()
        {
            if (RGSettings.reading && !isReading)
            {
                audioSource.clip = sexClip;

                isReading = true;
                audioSource.Play();
            }
        }
        public void ReadDelete()
        {
            if (RGSettings.reading && !isReading)
            {
                audioSource.clip = deleteClip;

                isReading = true;
                audioSource.Play();
            }
        }
        public void ReadSave()
        {
            if (RGSettings.reading && !isReading)
            {
                audioSource.clip = saveClip;

                isReading = true;
                audioSource.Play();
            }
        }

        public void SetAndReadSexValue()
        {
            sexInput.GetComponentInChildren<Text>().text = options[sexInput.value];

            if (RGSettings.reading && !isReading)
            {
                switch (sexInput.value)
                {
                    case (int)Gender.male:
                        audioSource.clip = maleClip;
                        break;

                    case (int)Gender.female:
                        audioSource.clip = femaleClip;
                        break;
                }


                isReading = true;
                audioSource.Play();
            }
        }
    }
}