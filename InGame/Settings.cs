namespace HSA.RehaGame.InGame
{
    using System;
    using Scene;
    using UnityEngine;

    [RequireComponent(typeof(SettingsAccessManager))]
    public class Settings : MonoBehaviour
    {
        public GameObject gameManager;

        private SettingsAccessManager settingsAccessManager;
        private SceneManager sceneManager;
        private static int languageIndex = -1;

        void Start()
        {
            settingsAccessManager = this.GetComponent<SettingsAccessManager>();
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        [Setting()]
        public string[] languages
        {
            get
            {
                return settingsAccessManager.Get("languages") as string[];
            }

            set
            {
                settingsAccessManager.Set("languages", value);
            }
        }

        [Setting()]
        public string activeLanguage
        {
            get
            {
                var language = settingsAccessManager.Get("activeLanguage") as string;

                if (languageIndex == -1)
                {
                    for (int i = 0; i < languages.Length; i++)
                    {
                        if (language == languages[i])
                        {
                            languageIndex = i;
                            break;
                        }
                    }
                }

                return language;
            }

            set
            {
                settingsAccessManager.Set("activeLanguage", value);
            }
        }

        [Setting()]
        public bool reading
        {
            get
            {
                var aloud = settingsAccessManager.Get("reading").ToString().ToLower();
                return aloud == "true";
            }

            set
            {
                settingsAccessManager.Set("reading", value);
            }
        }

        [Setting()]
        public bool music
        {
            get
            {
                var play = settingsAccessManager.Get("music").ToString().ToLower();
                return play == "true";
            }

            set
            {
                settingsAccessManager.Set("music", value);
            }
        }

        [Setting()]
        public double angleTolerance
        {
            get
            {
                return double.Parse(settingsAccessManager.Get("angleTolerance").ToString().ToLower());
            }

            set
            {
                settingsAccessManager.Set("angleTolerance", value);
            }
        }

        public string GetByPropertyName(object caller, string name)
        {
            var properties = Type.GetType("HSA.RehaGame.InGame.RGSettings").GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == name)
                {
                    var attr = property.GetCustomAttributes(typeof(Setting), true);

                    if (attr.Length == 1)
                    {
                        var get = property.GetGetMethod();
                        var result = get.Invoke(caller, null);

                        return result.ToString().ToLower();
                    }
                }
            }

            return "";
        }

        public void SwapLanguage()
        {
            languageIndex = ++languageIndex % languages.Length;
            activeLanguage = languages[languageIndex];
            Save();

            sceneManager.ReloadSettings();
        }

        public void SwapReadingAloud()
        {
            reading = !reading;
            Save();

            sceneManager.ReloadSettings();
        }

        public void SwapMusic()
        {
            music = !music;
            Save();

            sceneManager.ReloadSettings();
        }

        public void Save()
        {
            settingsAccessManager.Save();
        }
    }
}