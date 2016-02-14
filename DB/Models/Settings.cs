namespace HSA.RehaGame.DB.Models
{
    using System;
    using System.Collections.Generic;

    public class Settings : Model
    {
        private string group;
        private Dictionary<string, SettingsKeyValue> keyValue;

        public Settings(string group)
        {
            this.group = group;
        }

        [PrimaryKey]
        public string Group
        {
            get
            {
                return this.group;
            }

            private set
            {
                this.group = value;
            }
        }

        [ManyToManyRelation(
            "group",
            "settings",
            "settings_id",
            "settings_setting",
            "settingskeyvalue_id",
            "settingskeyvalue",
            "key"
        )]
        public Dictionary<string, SettingsKeyValue> KeyValue
        {
            get
            {
                return this.keyValue;
            }

            set
            {
                this.keyValue = value;
            }
        }

        public T GetValue<T>(string key)
        {
            if (keyValue.ContainsKey(key))
                return (T)keyValue[key].Value;

            throw new Exception(string.Format("Setting with key '{0}' not found", key));
        }
    }
}
