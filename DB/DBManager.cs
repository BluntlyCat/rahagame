namespace HSA.RehaGame.DB
{
    using UnityEngine;
    using System;
    using System.Data;
    using System.Linq;
    using Mono.Data.Sqlite;
    using System.Collections.Generic;
    using InGame;

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

        private string GetPKName(string table)
        {
            var info = Query("PRAGMA table_info(" + table + ");");

            foreach (var entry in info)
            {
                if (int.Parse(entry["pk"].ToString()) == 1)
                    return entry["name"].ToString();
            }

            return null;
        }

        private KeyValuePair<string, object> GetLastEntry(string table, string pkName, KeyValuePair<string, object>[] values)
        {
            foreach (var kv in values)
            {
                if (kv.Key == pkName)
                    return kv;
            }

            int seq = int.Parse(Query("SELECT seq FROM sqlite_sequence WHERE name='" + table + "';").First().First().Value.ToString());
            return new KeyValuePair<string, object>(pkName, seq);
        }

        private KeyValuePair<string, object> GetNextEntry(string table, string pkName, KeyValuePair<string, object>[] values)
        {
            foreach (var kv in values)
            {
                if (kv.Key == pkName)
                    return kv;
            }

            int seq = int.Parse(Query("SELECT seq FROM sqlite_sequence WHERE name='" + table + "';").First().First().Value.ToString()) + 1;
            return new KeyValuePair<string, object>(pkName, seq);
        }

        private string GetInsertValues(object[] values)
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

        private string GetInsertValueParams(string pkName, KeyValuePair<string, object>[] values)
        {
            var sql = " VALUES(@" + pkName + ", ";

            for (int i = 0; i < values.Length; i++)
            {
                if (pkName != values[i].Key)
                {
                    if (i < values.Length - 1)
                    {
                        sql += "@" + values[i].Key + ", ";
                    }
                    else
                    {
                        sql += "@" + values[i].Key + ")";
                    }
                }
            }

            return sql;
        }

        private string GetInsertColumnValues(KeyValuePair<string, object>[] columnValues)
        {
            var columns = " (";
            var values = " VALUES(";

            for (int i = 0; i < columnValues.Length; i++)
            {
                if (i < columnValues.Length - 1)
                {
                    columns += columnValues[i].Key + ", ";
                    values += "'" + columnValues[i].Value + "', ";
                }
                else
                {
                    columns += columnValues[i].Key + ")";
                    values += "'" + columnValues[i].Value + "');";
                }
            }

            return columns + values;
        }

        private string GetUpdateValues(KeyValuePair<string, object>[] values)
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

        public static IList<Dictionary<string, object>> Join(string fromTable, string joinTable, string joinTableCol, string fromTableCol, string matchCol, string matchValue)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var sql = "SELECT *"
                + "FROM " + fromTable
                + "JOIN " + joinTable + " ON " + joinTable + "." + joinTableCol + " = " + fromTable + "." + fromTableCol
                + " WHERE " + fromTable + "." + matchCol + "= " + matchValue + ";";

                IList<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();

                manager.dbcmd.CommandText = sql;
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

        public static IDictionary<string, object> GetMenuHeader(string name)
        {
            var header = Query("SELECT name, auditiveName FROM editor_menu WHERE unityObjectName = '" + name + "';").First();

            var text = header["name"];
            var audio = header["auditiveName"].ToString().Replace(".mp3", "").Replace("Assets/Resources/", "");

            return new Dictionary<string, object> {
                {"name", text },
                { "clip", Resources.Load(audio) }
            };
        }

        public static IDictionary<string, object> GetTranslation(string name)
        {
            var translation = Query("SELECT translation, auditiveTranslation FROM editor_valuetranslation WHERE unityObjectName = '" + name + "';").First();

            var text = translation["translation"];
            var audio = translation["auditiveTranslation"].ToString().Replace(".mp3", "").Replace("Assets/Resources/", "");

            return new Dictionary<string, object> {
                {"translation", text },
                { "clip", Resources.Load(audio) }
            };
        }

        public static string Insert(string table, params KeyValuePair<string, object>[] values)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var pkName = manager.GetPKName(table);
                var sql = @"INSERT INTO " + table + manager.GetInsertValueParams(pkName, values);
                var pk = manager.GetNextEntry(table, pkName, values);
                var pkParam = new SqliteParameter(pk.Key, pk.Value);

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();
                manager.dbcmd.CommandText = sql;

                manager.dbcmd.Parameters.Add(pkParam);

                foreach (KeyValuePair<string, object> value in values)
                {
                    bool canAdd = true;
                    var param = new SqliteParameter(value.Key, value.Value);

                    foreach (var p in manager.dbcmd.Parameters)
                    {
                        var addedParam = (SqliteParameter)p;

                        if (addedParam.ParameterName == param.ParameterName && addedParam.Value == param.Value)
                        {
                            canAdd = false;
                            break;
                        }
                    }

                    if (canAdd)
                        manager.dbcmd.Parameters.Add(param);
                }

                manager.reader = manager.dbcmd.ExecuteReader();

                manager.Close();

                var all = Query("SELECT * FROM " + table + ";");
                var lastId = manager.GetLastEntry(table, pkName, values);

                return lastId.Value.ToString();
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }

            finally
            {
                manager.dbconn.Close();
            }
        }

        public static object Update(string pk, string table, params KeyValuePair<string, object>[] values)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var sql = "UPDATE " + table + manager.GetUpdateValues(values) + " WHERE " + manager.GetPKName(table) + " = '" + pk + "';";

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();
                manager.dbcmd.CommandText = sql;
                manager.reader = manager.dbcmd.ExecuteReader();

                manager.Close();

                var all = Query("SELECT * FROM " + table + ";");

                return from e in all.Last() where e.Key == manager.GetPKName(table) select e.Key;
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
                var sql = "SELECT * FROM " + table + " WHERE " + manager.GetPKName(table) + " = '" + pk + "';";
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
                var sql = "DELETE FROM " + table + " WHERE " + manager.GetPKName(table) + " = '" + pk + "';";

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