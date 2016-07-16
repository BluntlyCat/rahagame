namespace HSA.RehaGame.UI.Menu
{
    using DB.Models;
    using Manager;
    using Manager.Audio;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class PatientSelectionMenu : MonoBehaviour
    {
        public GameObject gameManager;

        public GameObject menuTitleObject;
        public GameObject[] menuEntries;

        public GameObject patientButtonPrefab;
        public GameObject patientButtonStorage;

        private SceneManager sceneManager;
        private SoundManager soundManager;
        private MusicManager musicManager;

        private Menu menu;
        private IDictionary<object, Patient> patients;

        void Start()
        {
            sceneManager = gameManager.GetComponentInChildren<SceneManager>();
            soundManager = gameManager.GetComponentInChildren<SoundManager>();
            musicManager = gameManager.GetComponentInChildren<MusicManager>();

            patients = Model.All<Patient>();

            if (patients.Count == 0)
                sceneManager.LoadNewPatientMenu();

            menu = Model.GetModel<Menu>(this.name);
            menuTitleObject.GetComponent<Text>().text = menu.Name;

            foreach (var menuEntry in this.menuEntries)
            {
                menuEntry.GetComponentInChildren<Text>().text = menu.Entries[menuEntry.name].Entry;
            }

            soundManager.Enqueue(menu.AuditiveName);
            musicManager.AddMusic(menu.Music);
            CreatePatientButtons();
        }

        private void CreatePatientButtons()
        {
            bool isFirst = true;

            foreach (var patient in patients.Values)
            {
                var button = Instantiate(patientButtonPrefab);
                var text = button.GetComponentInChildren<Text>();
                var name = patient.Name;

                button.transform.SetParent(patientButtonStorage.transform, false);
                button.name = name;
                text.text = name;

                if (isFirst)
                {
                    var eventSystem = GameObject.Find("EventSystem");
                    eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = button.gameObject;
                    isFirst = false;
                }
            }
        }

        public void ReadMenu(GameObject menuEntry)
        {
            soundManager.Enqueue(menu.Entries[menuEntry.name].AuditiveEntry, true);
        }
    }
}
