namespace HSA.RehaGame.DB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Logging;

    public class DBTable
    {
        private string name;
        private int rowCount;

        private IList<DBTableRow> tableRows = new List<DBTableRow>();

        private static Logger<DBTable> logger = new Logger<DBTable>();

        public DBTable(string name)
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.name = name;
            this.rowCount = 0;
        }

        public DBTableRow GetRow()
        {
            return tableRows.First();
        }

        public DBTableRow GetRow(int row)
        {
            if (row < tableRows.Count)
                return tableRows[row];

            return null;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int RowCount
        {
            get
            {
                return rowCount;
            }
        }

        public IList<DBTableRow> Rows
        {
            get
            {
                return tableRows;
            }
        }

        public void AddRow(DBTableRow row)
        {
            tableRows.Add(row);
            rowCount++;
        }

        public string GetValue(string pk)
        {
            logger.Debug(pk);
            return tableRows.First().GetValue(pk);
        }

        public string GetValue(int row, string pk)
        {
            if (row < tableRows.Count)
                return tableRows[row].GetValue(pk);
            else
                throw new Exception();
        }
        public string GetValueFromLanguage(string pk)
        {
            return tableRows.First().GetValueFromLanguage(pk);
        }

        public string GetValueFromLanguage(int row, string pk)
        {
            if (row < tableRows.Count)
                return tableRows[row].GetValueFromLanguage(pk);
            else
                throw new Exception();
        }
        public T GetResource<T>(string column, string mime, bool lang = true) where T : UnityEngine.Object
        {
            return tableRows.First().GetResource<T>(column, mime, lang);
        }

        public T GetResource<T>(int row, string column, string mime, bool lang = true) where T : UnityEngine.Object
        {
            if (row < tableRows.Count)
                return tableRows[row].GetResource<T>(column, mime, lang);
            else
                throw new Exception();
        }

        public bool GetBool(string pk)
        {
            return tableRows.First().GetBool(pk);
        }

        public bool GetBool(int row, string pk)
        {
            if (row < tableRows.Count)
                return tableRows[row].GetBool(pk);
            else
                throw new Exception();
        }

        public int GetInt(string pk)
        {
            return tableRows.First().GetInt(pk);
        }

        public int GetInt(int row, string pk)
        {
            if (row < tableRows.Count)
                return tableRows[row].GetInt(pk);
            else
                throw new Exception();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, RowCount);
        }
    }
}
