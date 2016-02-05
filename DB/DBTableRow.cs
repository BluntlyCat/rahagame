namespace HSA.RehaGame.DB
{
    using System.Collections.Generic;
    using System.Linq;
    using InGame;

    public class DBTableRow
    {
        private DBTable table;
        private string pkName;
        private int columnCount;

        private Dictionary<string, DBTableColumn> tableColumns = new Dictionary<string, DBTableColumn>();

        public DBTableRow(DBTable table, string pkName)
        {
            this.table = table;
            this.pkName = pkName;
            this.columnCount = 0;
        }

        public DBTableColumn GetColumn()
        {
            return tableColumns.First().Value;
        }

        public DBTableColumn GetColumn(string pk)
        {
            if (tableColumns.ContainsKey(pk))
                return tableColumns[pk];

            return null;
        }

        public bool AddColumn(string pk, DBTableColumn column)
        {
            if (tableColumns.ContainsKey(pk) == false)
            {
                tableColumns.Add(pk, column);
                columnCount++;

                return true;
            }

            return false;
        }

        public string GetValue(string pk)
        {
            return tableColumns[pk].GetValue();
        }

        public T GetColumn<T>(string pk)
        {
            return tableColumns[pk].GetColumn<T>();
        }

        public string GetValueFromLanguage(string pk)
        {
            return tableColumns[string.Format("{0}_{1}", pk, RGSettings.activeLanguage)].GetValue();
        }

        public T GetResource<T>(string column, string mime, bool lang = true) where T : UnityEngine.Object
        {
            string key = lang ? string.Format("{0}_{1}", column, RGSettings.activeLanguage) : column;
            return tableColumns[key].GetResource<T>(mime);
        }

        public bool GetBool(string pk)
        {
            return tableColumns[pk].GetBool();
        }

        public int GetInt(string pk)
        {
            return tableColumns[pk].GetInt();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", tableColumns[pkName].Value, ColumnCount);
        }

        public DBTable Table
        {
            get
            {
                return table;
            }
        }

        public string PrimaryKey
        {
            get
            {
                return pkName;
            }
        }

        public int ColumnCount
        {
            get
            {
                return columnCount;
            }
        }
    }
}
