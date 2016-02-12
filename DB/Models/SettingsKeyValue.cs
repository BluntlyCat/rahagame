namespace HSA.RehaGame.DB.Models
{
    public class SettingsKeyValue : Model
    {
        private string key;
        private object value;

        public SettingsKeyValue(string key)
        {
            this.key = key;
        }

        [PrimaryKey]
        public string Key
        {
            get
            {
                return this.key;
            }

            private set
            {
                this.key = value;
            }
        }

        [TableColumn]
        public object Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}
