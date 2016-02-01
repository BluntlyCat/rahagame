namespace HSA.RehaGame.InGame
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using DB;

    public class SettingsManager
    {
        private static SettingsManager manager;

        private StringReader reader;
        private Dictionary<string, object> settings = new Dictionary<string, object>();

        private SettingsManager()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            var groupID = DBManager.Query("editor_settings", "SELECT id FROM editor_settings WHERE \"group\" = 'ingame'").GetValue("id");
            var settingsIDs = DBManager.Query("editor_settings_setting", "SELECT settingskeyvalue_id FROM editor_settings_setting WHERE settings_id = '" + groupID + "'");

            foreach (var row in settingsIDs.Rows)
            {
                var settingEntry = DBManager.Query("editor_settingskeyvalue", "SELECT key, value FROM editor_settingskeyvalue WHERE id = '" + row.GetValue("settingskeyvalue_id") + "'");
                var setting = settingEntry.GetRow();

                var key = setting.GetValue("key");
                var value = setting.GetValue("value");

                if (value.Contains(","))
                    settings.Add(key, value.Split(','));
                else
                    settings.Add(key, value);
            }
        }

        public static object Get(string key)
        {
            if (manager == null)
                manager = new SettingsManager();

            return manager.settings[key];
        }

        public static void Set(string key, object value)
        {
            if (manager == null)
                manager = new SettingsManager();

            manager.settings[key] = value;
        }

        public static void Save()
        {
            if (manager != null)
            {
                var properties = Type.GetType("HSA.RehaGame.InGame.RGSettings").GetProperties();

                foreach (var property in properties)
                {
                    var attr = property.GetCustomAttributes(typeof(Setting), true);

                    if (attr.Length == 1)
                    {
                        var get = property.GetGetMethod();
                        var result = get.Invoke(manager, null);

                        if (result.GetType() == typeof(string[]))
                        {
                            var slots = "";

                            foreach (var slot in result as string[])
                            {
                                slots += slot.ToString() + ",";
                            }

                            result = slots.Substring(0, slots.Length - 1);
                        }

                        DBManager.Query("editor_settingskeyvalue", "UPDATE editor_settingskeyvalue SET value='" + result.ToString().ToLower() + "' WHERE key = '" + property.Name + "'");
                    }
                }
            }
        }
    }
}
