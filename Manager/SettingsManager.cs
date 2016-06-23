namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SettingsManager : MonoBehaviour
    {
        private IDictionary<object, Settings> models;

        void Awake()
        {
            models = Model.All<Settings>();
        }


        public T GetValue<T>(string group, string key)
        {
            return this.models[group].GetValue<T>(key);
        }

        public void SetValue<T>(string group, string key, T value)
        {
            this.models[group].SetValue(key, value);
        }
        
        public void SwitchBooleanType(GameObject button)
        {

        }

        public SettingsKeyValue GetKeyValue(string group, string key)
        {
            if (models.ContainsKey(group))
            {
                var setting = this.models[group];

                if(setting.KeyValue.ContainsKey(key))
                    return setting.KeyValue[key];
            }

            throw new Exception(string.Format("No setting with key '{0}' found", key));
        }
    }
}
