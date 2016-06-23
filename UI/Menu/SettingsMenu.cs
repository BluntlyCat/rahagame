namespace HSA.RehaGame.UI.Menu
{
    using DB.Models;
    using Manager;
    using Manager.Audio;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class SettingsMenu : MonoBehaviour
    {
        public GameObject gameManager;

        public GameObject menuTitleObject;
        public GameObject[] menuEntries;

        private SettingsManager settingsManager;
        private SoundManager soundManager;
        private MusicManager musicManager;

        private Menu menu;

        void Start()
        {
            menu = Model.GetModel<Menu>(this.name);

            settingsManager = gameManager.GetComponentInChildren<SettingsManager>();
            soundManager = gameManager.GetComponentInChildren<SoundManager>();
            musicManager = gameManager.GetComponentInChildren<MusicManager>();

            menuTitleObject.GetComponent<Text>().text = menu.Name;

            foreach (var menuEntry in this.menuEntries)
            {
                SetEntry(menuEntry);
            }

            soundManager.Enqueue(menu.AuditiveName);
            musicManager.AddMusic(menu.Music);
        }

        public void ChangeSetting(GameObject gameObject)
        {
            var keyValue = settingsManager.GetKeyValue("ingame", gameObject.name);

            if (keyValue.Type == typeof(bool))
                settingsManager.SetValue("ingame", gameObject.name, !keyValue.GetValue<bool>());

            else if(keyValue.Type == typeof(string))
                settingsManager.SetValue("ingame", gameObject.name, keyValue.GetValue<string>());

            else if (keyValue.Type == typeof(int))
                settingsManager.SetValue("ingame", gameObject.name, keyValue.GetValue<int>());

            else if (keyValue.Type == typeof(List<object>))
                settingsManager.SetValue("ingame", gameObject.name, keyValue.GetValue<string>());

        }

        public void ReadSetting(GameObject menuEntry)
        {
            if (settingsManager.GetValue<bool>("ingame", "reading"))
            {
                var value = GetValue(menuEntry);
                var valueTranslation = Model.GetModel<ValueTranslation>(value);

                soundManager.Enqueue(menu.Entries[menuEntry.name].AuditiveEntry);
                soundManager.Enqueue(valueTranslation.AuditiveTranslation);
            }
        }

        public void SetEntry(GameObject menuEntry)
        {
            var entry = menu.Entries[menuEntry.name].Entry;
            var value = GetValue(menuEntry);
            var valueTranslation = Model.GetModel<ValueTranslation>(value);

            menuEntry.GetComponent<Text>().text = string.Format("{0}: {1}", entry, valueTranslation.Translation);
        }

        private object GetValue(GameObject gameObject)
        {
            var keyValue = settingsManager.GetKeyValue("ingame", gameObject.name);
            
            return keyValue.Value;
        }
    }
}
