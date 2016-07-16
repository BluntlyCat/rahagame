namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SettingsManager : MonoBehaviour
    {
        private static IDictionary<object, Settings> settings;

        private static IDictionary<object, Settings> Settings
        {
            get
            {
                if (settings == null)
                    settings = Model.All<Settings>();

                return settings;
            }
        }


        public T GetValue<T>(string group, string key)
        {
            return Settings[group].GetValue<T>(key);
        }

        public void SetValue<T>(string group, string key, T value)
        {
            Settings[group].SetValue(key, value);
        }
        
        public void SwitchBooleanType(GameObject button)
        {

        }

        public SettingsKeyValue GetKeyValue(string group, string key)
        {
            if (Settings.ContainsKey(group))
            {
                var setting = Settings[group];

                if(setting.KeyValue.ContainsKey(key))
                    return setting.KeyValue[key];
            }

            throw new Exception(string.Format("No setting with key '{0}' found", key));
        }
    }
}
