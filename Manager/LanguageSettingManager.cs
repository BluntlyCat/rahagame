namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using System.Collections;
    using UnityEngine;

    public class LanguageSettingManager : MonoBehaviour
    {
        private static SettingsKeyValue activeLanguange;

        private SettingsManager settingsManager;
        private SceneManager sceneManager;
        private IList languages;

        void Start()
        {
            settingsManager = GetComponent<SettingsManager>();
            sceneManager = GetComponent<SceneManager>();
            activeLanguange = settingsManager.GetKeyValue("ingame", "activeLanguage");
            languages = settingsManager.GetKeyValue("ingame", "languages").GetValue<IList>();
        }

        public void SwitchLanguage()
        {
            int index = 0;

            foreach (var language in languages)
            {
                index++;

                if (language.ToString() == activeLanguange.GetValue<string>())
                    break;
            }

            var newLanguage = languages[index % languages.Count].ToString();
            activeLanguange.SetValue<string>(newLanguage);

            sceneManager.ReloadSettings();
        }

        public static string GetActiveLanguage()
        {
            if (activeLanguange == null)
                return "de_de";

            return activeLanguange.Value.ToString();
        }
    }
}
