namespace HSA.RehaGame.DB
{
    public class FieldValuePair
    {
        private string name;
        private object value;

        public FieldValuePair(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public object Value
        {
            get
            {
                return value;
            }
        }
    }
}
