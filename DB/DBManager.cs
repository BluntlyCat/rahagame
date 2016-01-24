namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System;
    using System.Data;
    using System.Linq;
    using Mono.Data.Sqlite;
    using System.Collections.Generic;
    using Settings;

    public class DBManager
    {
        private static DBManager manager;

        private IDbConnection dbconn;
        private IDbCommand dbcmd;
        private IDataReader reader;

        private string dbFile;

        private DBManager()
        {
            dbFile = "URI=file:" + Application.dataPath + "/StreamingAssets/db/rgdbe.db";
        }

        private void Open()
        {
            dbconn = new SqliteConnection(dbFile);
            dbconn.Open();
        }

        private void Close()
        {
            reader.Close();
            reader = null;

            dbcmd.Dispose();
            dbcmd = null;

            dbconn.Close();
        }

        private string GetID(string table)
        {
            var info = Query("PRAGMA table_info(" + table + ");");

            foreach(var entry in info)
            {
                if (int.Parse(entry["pk"].ToString()) == 1)
                    return entry["name"].ToString();
            }

            return null;
        }

        private string GetInsertValues(params object[] values)
        {
            var sql = " VALUES(";

            for (int i = 0; i < values.Length; i++)
            {
                if (i < values.Length - 1)
                {
                    sql += "'" + values[i].ToString() + "', ";
                }
                else
                {
                    sql += "'" + values[i].ToString() + "');";
                }
            }

            return sql;
        }

        private string GetUpdateValues(params KeyValuePair<string, object>[] values)
        {
            var sql = " SET ";

            for (int i = 0; i < values.Length; i++)
            {
                if (i < values.Length - 1)
                {
                    sql += values[i].Key + " = '" + values[i].Value + "', ";
                }
                else
                {
                    sql += values[i].Key + " = '" + values[i].Value + "' ";
                }
            }

            return sql;
        }

        private static void Instaciate()
        {
            manager = new DBManager();
        }

        public static IList<Dictionary<string, object>> Query(string query)
        {
            if (manager == null)
                Instaciate();

            try
            {
                IList<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();

                manager.dbcmd.CommandText = query;
                manager.reader = manager.dbcmd.ExecuteReader();

                while (manager.reader.Read())
                {
                    var dict = new Dictionary<string, object>();

                    for (int i = 0; i < manager.reader.FieldCount; i++)
                    {
                        var name = manager.reader.GetName(i);
                        var value = manager.reader.GetValue(i);

                        dict.Add(name, value);
                    }

                    if (dict.Count > 0)
                        list.Add(dict);
                }

                manager.Close();

                return list;
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }

        public static string Query(string column, string table, string unityObjectName)
        {
            if (manager == null)
                Instaciate();

            try
            {
                string value;
                string language = RGSettings.activeLanguage;
                string sql = "SELECT " + column + "_" + language + " FROM " + table + " WHERE unityObjectName = '" + unityObjectName + "'";

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();
                manager.dbcmd.CommandText = sql;

                manager.reader = manager.dbcmd.ExecuteReader();

                value = manager.reader.GetValue(0).ToString();

                manager.Close();

                return value;
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }

        public static Dictionary<string, object> GetMenuHeader(string name)
        {
            var header = Query("SELECT name, auditiveName FROM editor_menu WHERE unityObjectName = '" + name + "';").First();

            var text = header["name"];
            var audio = header["auditiveName"].ToString().Replace(".mp3", "").Replace("Assets/Resources/", "");

            return new Dictionary<string, object> {
                {"name", text },
                { "clip", Resources.Load(audio) }
            };
        }

        public static object Insert(string table, params object[] values)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var sql = "INSERT INTO " + table + manager.GetInsertValues(values);

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();
                manager.dbcmd.CommandText = sql;
                manager.reader = manager.dbcmd.ExecuteReader();

                manager.Close();

                var all = Query("SELECT * FROM " + table + ";");

                return from e in all.Last() where e.Key == manager.GetID(table) select e.Key;
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }

        public static object Update(string pk, string table, params KeyValuePair<string, object>[] values)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var sql = "UPDATE " + table + manager.GetUpdateValues(values) + " WHERE " + manager.GetID(table) + " = '" + pk + "';";

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();
                manager.dbcmd.CommandText = sql;
                manager.reader = manager.dbcmd.ExecuteReader();

                manager.Close();

                var all = Query("SELECT * FROM " + table + ";");

                return from e in all.Last() where e.Key == manager.GetID(table) select e.Key;
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }

        public static bool Exists(string table, string pk)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var sql = "SELECT * FROM " + table + " WHERE " + manager.GetID(table) + " = '" + pk + "';";
                bool exists;

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();
                manager.dbcmd.CommandText = sql;
                manager.reader = manager.dbcmd.ExecuteReader();
                exists = manager.reader.Read();

                manager.Close();

                return exists;
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }

        public static void Detete(string table, string pk)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var sql = "DELETE FROM " + table + " WHERE " + manager.GetID(table) + " = '" + pk + "';";

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();
                manager.dbcmd.CommandText = sql;
                manager.reader = manager.dbcmd.ExecuteReader();

                manager.Close();
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }
    }
}