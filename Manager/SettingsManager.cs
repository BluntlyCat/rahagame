namespace HSA.RehaGame.Manager
{
    using System;
    using System.Collections.Generic;
    using DB.Models;

    public class SettingsManager : BaseModelManager<Settings>
    {
        private IDictionary<string, SettingsKeyValue> keyValues;

        public SettingsManager(object group)
        {
            keyValues = models[group].KeyValue;
        }

        public T Value<T>(string key)
        {
            if (keyValues.ContainsKey(key))
                return (T)keyValues[key].Value;

            throw new Exception(string.Format("No setting with key '{0}' found", key));
        }
    }
}
