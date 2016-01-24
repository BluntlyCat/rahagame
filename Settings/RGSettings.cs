namespace HSA.RehaGame.Settings
{
    using System;
    using DB;
    using User;
    using Scene;
    using UnityEngine;

    public class RGSettings : MonoBehaviour
    {
        private static int languageIndex = -1;
        private static Patient activePatient;

        [Setting(true)]
        public static string[] languages
        {
            get
            {
                return SettingsManager.Get("languages") as string[];
            }

            set
            {
                SettingsManager.Set("languages", value);
            }
        }

        [Setting(true)]
        public static string activeLanguage
        {
            get
            {
                var language = SettingsManager.Get("activeLanguage") as string;

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
                SettingsManager.Set("activeLanguage", value);
            }
        }

        [Setting(true)]
        public static bool readingAloud
        {
            get
            {
                var aloud = SettingsManager.Get("readingAloud").ToString().ToLower();
                return aloud == "true";
            }

            set
            {
                SettingsManager.Set("readingAloud", value);
            }
        }

        [Setting(true)]
        public static bool music
        {
            get
            {
                var play = SettingsManager.Get("music").ToString().ToLower();
                return play == "true";
            }

            set
            {
                SettingsManager.Set("music", value);
            }
        }

        public static Patient ActivePatient
        {
            get
            {
                return activePatient;
            }

            set
            {
                activePatient = value;
            }
        }

        public static string GetTranslation(string name)
        {
            return DBManager.Query("translation", "editor_valuetranslation", name);
        }

        public static string GetByPropertyName(object caller, string name)
        {
            var properties = Type.GetType("HSA.RehaGame.Settings.RGSettings").GetProperties();

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

            LoadScene.ReloadSettings();
        }

        public void SwapReadingAloud()
        {
            readingAloud = !readingAloud;
            Save();

            LoadScene.ReloadSettings();
        }

        public void SwapMusic()
        {
            music = !music;
            Save();

            LoadScene.ReloadSettings();
        }

        public static void Save()
        {
            SettingsManager.Save();
        }
    }
}