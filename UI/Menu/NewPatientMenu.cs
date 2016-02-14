namespace HSA.RehaGame.UI.Menu
{
    using System.Collections.Generic;
    using DB.Models;
    using Manager;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class NewPatientMenu : MonoBehaviour
    {
        public GameObject gameManager;

        public SoundManager soundManager;

        public GameObject menuTitleObject;
        public GameObject nameInputObject;
        public GameObject nameLabelObject;

        public GameObject ageInputObject;
        public GameObject ageLabelObject;

        public GameObject sexInputObject;
        public GameObject sexLabelObject;

        public GameObject saveButtonObject;
        public GameObject deleteButtonObject;

        private Text nameText;
        private Text namePlaceholder;

        private Text ageText;
        private Text agePlaceholder;

        private Dropdown sexInput;

        private AudioClip nameClip;
        private AudioClip ageClip;
        private AudioClip sexClip;
        private AudioClip deleteClip;
        private AudioClip saveClip;
        private AudioClip maleClip;
        private AudioClip femaleClip;

        private List<string> options;

        private Patient patient;

        void Start()
        {
            var menu = Model.GetModel<Menu>(this.name);
            var male = Model.GetModel<ValueTranslation>("male");
            var female = Model.GetModel<ValueTranslation>("female");

            soundManager = gameManager.transform.Find("SoundManager").GetComponent<SoundManager>();

            menuTitleObject.GetComponent<Text>().text = menu.Name;
            nameLabelObject.GetComponent<Text>().text = string.Format("{0}: ", menu.Entries["name"].Entry);
            ageLabelObject.GetComponent<Text>().text = string.Format("{0}: ", menu.Entries["age"].Entry);
            sexLabelObject.GetComponent<Text>().text = string.Format("{0}: ", menu.Entries["sex"].Entry);

            nameText = nameInputObject.transform.Find("nameText").GetComponent<Text>();
            namePlaceholder = nameInputObject.transform.Find("namePlaceholder").GetComponent<Text>();
            namePlaceholder.text = menu.Entries["name"].Placeholder;

            ageText = ageInputObject.transform.Find("ageText").GetComponent<Text>();
            agePlaceholder = ageInputObject.transform.Find("agePlaceholder").GetComponent<Text>();

            sexInput = sexInputObject.GetComponent<Dropdown>();
            options = new List<string>();
            options.Add(male.Translation);
            options.Add(female.Translation);
            sexInput.AddOptions(options);

            nameClip = menu.Entries["name"].AuditiveEntry;
            ageClip = menu.Entries["age"].AuditiveEntry;
            sexClip = menu.Entries["sex"].AuditiveEntry;
            deleteClip = menu.Entries["deletePatient"].AuditiveEntry;
            saveClip = menu.Entries["savePatient"].AuditiveEntry;
            maleClip = male.AuditiveTranslation;
            femaleClip = female.AuditiveTranslation;

            if (GameManager.ActivePatient != null)
            {
                patient = GameManager.ActivePatient;

                nameText.text = patient.Name;
                ageText.text = patient.Age.ToString();
                sexInput.value = (int)patient.Sex;
                sexInput.GetComponentInChildren<Text>().text = options[(int)patient.Sex];

                sexInput.RefreshShownValue();

                GameObject.Find("EventSystem").GetComponent<EventSystem>().firstSelectedGameObject = saveButtonObject;
            }
            else
                sexInput.GetComponentInChildren<Text>().text = options[0];
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
            soundManager.Enqueue(sexClip);
        }

        public void ReadDelete()
        {
            soundManager.Enqueue(deleteClip);
        }

        public void ReadSave()
        {
            soundManager.Enqueue(saveClip);
        }

        public void SetAndReadSexValue()
        {
            
        }
    }
}
