namespace HSA.RehaGame.DB
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using InGame;
    using Logging;
    using Mono.Data.Sqlite;
    using UnityEngine;
    using Windows.Kinect;

    public class DBManager
    {
        private static DBManager manager;

        private IDbConnection dbconn;
        private IDbCommand dbcmd;
        private IDataReader reader;

        private string dbFile;

        private static Logger<DBManager> logger = new Logger<DBManager>();

        private DBManager()
        {
            logger.AddLogAppender<ConsoleAppender>();
            dbFile = "Data Source=" + Application.dataPath + "/StreamingAssets/db/rgdbe.db";
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
            string pkName = null;

            manager.Open();

            manager.dbcmd = manager.dbconn.CreateCommand();

            manager.dbcmd.CommandText = "PRAGMA table_info(" + table + ");";
            manager.reader = manager.dbcmd.ExecuteReader();

            while (manager.reader.Read())
            {
                for (int i = 0; i < manager.reader.FieldCount; i++)
                {
                    var name = manager.reader.GetName(i).ToString();
                    var value = manager.reader.GetValue(i).ToString();

                    if (name == "pk" && int.Parse(value) == 1)
                    {
                        pkName = manager.reader.GetString(1);
                    }
                }
            }

            manager.Close();
            return pkName;
        }

        private KeyValuePair<string, object> GetLastEntry(string table, string pkName, KeyValuePair<string, object>[] values)
        {
            foreach (var kv in values)
            {
                if (kv.Key == pkName)
                    return kv;
            }

            int seq = Query("sqlite_sequence", "SELECT seq FROM sqlite_sequence WHERE name='" + table + "';").GetInt("seq");
            return new KeyValuePair<string, object>(pkName, seq);
        }

        private KeyValuePair<string, object> GetNextEntry(string table, string pkName, KeyValuePair<string, object>[] values)
        {
            foreach (var kv in values)
            {
                if (kv.Key == pkName)
                    return kv;
            }

            int seq = Query("sqlite_sequence", "SELECT seq FROM sqlite_sequence WHERE name='" + table + "';").GetInt("seq") + 1;
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

        private string GetUpdateValues(string pkName, KeyValuePair<string, object>[] values)
        {
            var sql = " SET " + pkName + " = :" + pkName + ", ";

            for (int i = 0; i < values.Length; i++)
            {
                if (pkName != values[i].Key)
                {
                    if (i < values.Length - 1)
                    {
                        sql += values[i].Key + " = :" + values[i].Key + ", ";
                    }
                    else
                    {
                        sql += values[i].Key + " = :" + values[i].Key + " ";
                    }
                }
            }

            return sql;
        }

        private static void Instaciate()
        {
            manager = new DBManager();
        }

        public static DBTable Query(string tableName, string query)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var table = new DBTable(tableName);
                var pkName = manager.GetPKName(tableName);

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();

                manager.dbcmd.CommandText = query;
                manager.reader = manager.dbcmd.ExecuteReader();

                while (manager.reader.Read())
                {
                    var row = new DBTableRow(table, pkName);

                    for (int i = 0; i < manager.reader.FieldCount; i++)
                    {
                        var type = manager.reader.GetFieldType(i);
                        var name = manager.reader.GetName(i);
                        var value = manager.reader.GetValue(i);

                        row.AddColumn(name, new DBTableColumn(row, type, name, value));
                    }

                    table.AddRow(row);
                }

                manager.Close();

                return table;
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }

        public static DBTableColumn Query(string column, string table, string unityObjectName)
        {
            if (manager == null)
                Instaciate();

            try
            {
                string language = RGSettings.activeLanguage;
                string sql = "SELECT " + column + "_" + language + " FROM " + table + " WHERE unityObjectName = '" + unityObjectName + "'";

                return Query(table, sql).GetRow().GetColumn();
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }

        public static DBTable GetPatientJoint(JointType kinectJoint_id, string patientName)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var sql = string.Format(@"
                    SELECT editor_patientjoint.id,
                    editor_patientjoint.active,
                    editor_patientjoint.x_axis_min_value,
                    editor_patientjoint.x_axis_max_value,
                    editor_patientjoint.y_axis_min_value,
                    editor_patientjoint.y_axis_max_value,
                    editor_patientjoint.z_axis_min_value,
                    editor_patientjoint.z_axis_max_value,
                    editor_patientjoint.kinectJoint_id
                    FROM editor_patientjoint
                    JOIN editor_patient_joints ON editor_patient_joints.patientjoint_id = editor_patientjoint.id
                    JOIN editor_patient ON editor_patient.name = editor_patient_joints.patient_id
                    WHERE editor_patientjoint.kinectJoint_id = '{0}' and editor_patient.name = '{1}';
                    ", kinectJoint_id, patientName);

                DBTable table = new DBTable("editor_patientjoint");

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();

                manager.dbcmd.CommandText = sql;
                manager.reader = manager.dbcmd.ExecuteReader();

                while (manager.reader.Read())
                {
                    var row = new DBTableRow(table, "id");

                    for (int i = 0; i < manager.reader.FieldCount; i++)
                    {
                        var type = manager.reader.GetFieldType(i);
                        var name = manager.reader.GetName(i);
                        var value = manager.reader.GetValue(i);

                        row.AddColumn(name, new DBTableColumn(row, type, name, value));
                    }

                    table.AddRow(row);
                }

                manager.Close();

                return table;
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }
        public static DBTable GetStressedJoints(int exerciseID)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var sql = string.Format(@"
                    SELECT editor_joint.name,
                    editor_joint.value,
                    editor_joint.translation,
                    editor_joint.translation_de_de,
                    editor_joint.translation_en_gb,
                    editor_joint.x_axis,
                    editor_joint.x_axis_min_value,
                    editor_joint.x_axis_max_value,
                    editor_joint.y_axis,
                    editor_joint.y_axis_min_value,
                    editor_joint.y_axis_max_value,
                    editor_joint.z_axis,
                    editor_joint.z_axis_min_value,
                    editor_joint.z_axis_max_value,
                    editor_joint.parent_id
                    FROM editor_joint
                    JOIN editor_exercise_stressedJoints ON editor_exercise_stressedJoints.joint_id = editor_joint.name
                    JOIN editor_exercise ON editor_exercise.id = editor_exercise_stressedJoints.exercise_id
                    WHERE editor_exercise.id = {0};
                    ", exerciseID);

                DBTable table = new DBTable("editor_joint");

                manager.Open();

                manager.dbcmd = manager.dbconn.CreateCommand();

                manager.dbcmd.CommandText = sql;
                manager.reader = manager.dbcmd.ExecuteReader();

                while (manager.reader.Read())
                {
                    var row = new DBTableRow(table, "name");

                    for (int i = 0; i < manager.reader.FieldCount; i++)
                    {
                        var type = manager.reader.GetFieldType(i);
                        var name = manager.reader.GetName(i);
                        var value = manager.reader.GetValue(i);

                        row.AddColumn(name, new DBTableColumn(row, type, name, value));
                    }

                    table.AddRow(row);
                }

                manager.Close();

                return table;
            }

            catch (Exception e)
            {
                Debug.Log(e);
                throw e;
            }
        }

        public static DBTable GetMenuHeader(string name)
        {
            return Query("editor_menu", "SELECT * FROM editor_menu WHERE unityObjectName = '" + name + "';");
        }

        public static DBTable GetExerciseInformation(string name, string type)
        {
            return Query("editor_exerciseinformation", "SELECT * FROM editor_exerciseinformation WHERE unityObjectName = '" + name + "' AND type = '" + type + "';");
        }

        public static DBTable GetTranslation(string name)
        {
            var table = Query("editor_valuetranslation", "SELECT * FROM editor_valuetranslation WHERE unityObjectName = '" + name + "';");

            return table;
        }

        public static DBTable GetTranslation(Logics logic, Orders order, params string[] names)
        {
            string expression = "unityObjectName = '{0}'";
            string WHERE = "WHERE ";

            for(int i = 0; i < names.Length; i++)
            {
                if(i == 0)
                    WHERE = string.Format("{0} {1} {2}", WHERE, string.Format(expression, names[i]), logic.ToString());
                else
                    WHERE = string.Format("{0} {1} ORDER BY unityObjectName {2}", WHERE, string.Format(expression, names[i]), order.ToString());
            }

            string query = string.Format("SELECT * FROM editor_valuetranslation {0}", WHERE);

            var table = Query("editor_valuetranslation", query);

            return table;
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

        public static string Update(string pk, string table, params KeyValuePair<string, object>[] values)
        {
            if (manager == null)
                Instaciate();

            try
            {
                var pkName = manager.GetPKName(table);
                var pkParam = new SqliteParameter(pkName, pk);
                var sql = @"UPDATE " + table + manager.GetUpdateValues(pkName, values) + "WHERE " + pkName + " = '" + pk + "';";

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

                return pk;
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