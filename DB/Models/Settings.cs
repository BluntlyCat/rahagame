namespace HSA.RehaGame.DB.Models
{
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
    }
}
