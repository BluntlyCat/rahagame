namespace HSA.RehaGame.DB
{
    using System;

    public class DBTableColumn
    {
        private DBTableRow row;

        private Type type;
        private string column;
        private object value;

        public DBTableColumn(DBTableRow row, Type type, string column, object value)
        {
            this.row = row;

            this.type = type;
            this.column = column;
            this.value = value;
        }

        public DBTableRow Row
        {
            get
            {
                return row;
            }
        }

        public Type ColumnType
        {
            get
            {
                return this.type;
            }
        }

        public string Column
        {
            get
            {
                return this.column;
            }
        }

        public object Value
        {
            get
            {
                return this.value;
            }
        }

        public string GetValue()
        {
            return this.Value.ToString();
        }public string GetResource(string mime)
        {
            return this.Value.ToString().Replace(string.Format(".{0}", mime), "").Replace("Assets/Resources/", "");
        }

        public bool GetBool()
        {
            return Convert.ToBoolean(this.Value);
        }

        public int GetInt()
        {
            return Convert.ToInt32(this.Value);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Value, ColumnType);
        }
    }
}
