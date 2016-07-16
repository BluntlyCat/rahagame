namespace HSA.RehaGame.UI.Menu
{
    using System.Collections.Generic;
    using DB.Models;
    using Manager.Audio;
    using Manager;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class NewPatientMenu : MonoBehaviour
    {
        public GameObject gameManager;
        
        public GameObject menuTitleObject;
        public GameObject nameInputObject;
        public GameObject nameLabelObject;

        public GameObject ageInputObject;
        public GameObject ageLabelObject;

        public GameObject sexInputObject;
        public GameObject sexLabelObject;

        public GameObject saveButtonObject;
        public GameObject statisticButtonObject;
        public GameObject deleteButtonObject;

        private InputField nameInput;
        private Text namePlaceholder;

        private InputField ageInput;

        private Dropdown sexInput;
        private Text sexInputText;

        private AudioClip nameClip;
        private AudioClip ageClip;
        private AudioClip sexClip;
        private AudioClip deleteClip;
        private AudioClip statisticClip;
        private AudioClip saveClip;
        private AudioClip maleClip;
        private AudioClip femaleClip;

        private List<string> options;

        private SceneManager sceneManager;
        private PatientManager patientManager;
        private SoundManager soundManager;
        private MusicManager musicManager;

        private ValueTranslation male;
        private ValueTranslation female;

        private bool valueChanged = false;

        void Start()
        {
            var menu = Model.GetModel<Menu>(this.name);
            male = Model.GetModel<ValueTranslation>("male");
            female = Model.GetModel<ValueTranslation>("female");

            soundManager = gameManager.GetComponentInChildren<SoundManager>();
            musicManager = gameManager.GetComponentInChildren<MusicManager>();
            patientManager = gameManager.GetComponent<PatientManager>();
            sceneManager = gameManager.GetComponent<SceneManager>();

            menuTitleObject.GetComponent<Text>().text = menu.Name;
            nameLabelObject.GetComponent<Text>().text = string.Format("{0}: ", menu.Entries["name"].Entry);
            ageLabelObject.GetComponent<Text>().text = string.Format("{0}: ", menu.Entries["age"].Entry);
            sexLabelObject.GetComponent<Text>().text = string.Format("{0}: ", menu.Entries["sex"].Entry);

            nameInput = nameInputObject.GetComponent<InputField>();
            namePlaceholder = nameInputObject.transform.Find("namePlaceholder").GetComponent<Text>();
            namePlaceholder.text = menu.Entries["name"].Placeholder;

            ageInput = ageInputObject.GetComponent<InputField>();

            sexInput = sexInputObject.GetComponent<Dropdown>();

            options = new List<string>();
            options.Add(male.Translation);
            options.Add(female.Translation);
            sexInput.AddOptions(options);

            nameClip = menu.Entries["name"].AuditiveEntry;
            ageClip = menu.Entries["age"].AuditiveEntry;
            sexClip = menu.Entries["sex"].AuditiveEntry;
            deleteClip = menu.Entries["deletePatient"].AuditiveEntry;
            statisticClip = menu.Entries["statistic"].AuditiveEntry;
            saveClip = menu.Entries["savePatient"].AuditiveEntry;
            maleClip = male.AuditiveTranslation;
            femaleClip = female.AuditiveTranslation;

            saveButtonObject.GetComponentInChildren<Text>().text = menu.Entries["savePatient"].Entry;
            statisticButtonObject.GetComponentInChildren<Text>().text = menu.Entries["statistic"].Entry;
            deleteButtonObject.GetComponentInChildren<Text>().text = menu.Entries["deletePatient"].Entry;

            soundManager.Enqueue(menu.AuditiveName);
            musicManager.AddMusic(menu.Music);


            if (GameManager.ActivePatient != null)
            {
                nameInput.text = GameManager.ActivePatient.Name;
                var age = GameManager.ActivePatient.Age.ToString();
                ageInput.text = age;
                sexInput.value = (int)GameManager.ActivePatient.Sex;

                sexInput.transform.FindChild("Label").GetComponent<Text>().text = GameManager.ActivePatient.Sex == Sex.male ? male.Translation : female.Translation;

                sexInput.RefreshShownValue();

                GameObject.Find("EventSystem").GetComponent<EventSystem>().firstSelectedGameObject = saveButtonObject;
            }
            else
            {
                sexInput.GetComponentInChildren<Text>().text = options[0];
                statisticButtonObject.SetActive(false);
                deleteButtonObject.SetActive(false);
            }
        }

        public void Save()
        {
            if (GameManager.ActivePatient != null)
            {
                GameManager.ActivePatient.Age = int.Parse(ageInput.text);
                GameManager.ActivePatient.Sex = (Sex)sexInput.value;

                GameManager.ActivePatient.Save();
            }
            else
                patientManager.AddPatient(nameInput.text, ageInput.text, sexInput.value);

            sceneManager.LoadNewPatientMenu();
        }

        public void Delete()
        {
            patientManager.DeletePatient();
            GameManager.ActivePatient = null;
            sceneManager.LoadPatientSlectionMenu();
        }

        public void ReadName()
        {
            soundManager.Enqueue(nameClip);
        }

        public void ReadAge()
        {
            soundManager.Enqueue(ageClip);
        }

        public void ReadSex()
        {
            if (valueChanged)
            {
                valueChanged = false;
                return;
            }

            soundManager.Enqueue(sexClip);
            SetAndReadSexValue();
        }

        public void ReadDelete()
        {
            soundManager.Enqueue(deleteClip);
        }

        public void ReadStatistic()
        {
            soundManager.Enqueue(statisticClip);
        }

        public void ReadSave()
        {
            soundManager.Enqueue(saveClip);
        }

        public void SetAndReadSexValue()
        {
            var sex = ((Sex)sexInput.value);

            sexInput.transform.FindChild("Label").GetComponent<Text>().text = sex == Sex.male ? male.Translation : female.Translation;
            
            if (sex == Sex.male)
                soundManager.Enqueue(maleClip);
            else
                soundManager.Enqueue(femaleClip);

            valueChanged = true;
        }
    }
}
