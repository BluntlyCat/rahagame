namespace HSA.RehaGame.DB
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Threading;
    using Models;
    using Mono.Data.Sqlite;
    using UnityEngine;

    public class Database : IDatabase
    {
        private static IDatabase database;
        private static Mutex mutex = new Mutex();

        private string tablePrefix;
        private IList<Type> knownModels;
        private IDbConnection dbConnection;

        private Database(string tablePrefix)
        {
            var dbFile = "Data Source=" + Application.dataPath + "/StreamingAssets/db/rgdbe.db";

            this.tablePrefix = tablePrefix;
            knownModels = new List<Type>();
            dbConnection = new SqliteConnection(dbFile);
        }

        ~Database()
        {
            dbConnection.Close();
            mutex.ReleaseMutex();
        }

        public static IDatabase Instance(string tablePrefix = "")
        {
            if (database == null)
                database = new Database(tablePrefix);

            return database;
        }

        private IDbCommand OpenConnection()
        {
            if (dbConnection.State == ConnectionState.Closed)
                dbConnection.Open();

            return dbConnection.CreateCommand();
        }

        private void CloseConnection()
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection.Close();
        }

        private SQLiteErrorCode WriteQuery(string query)
        {
            try
            {
                if (mutex.WaitOne())
                {
                    IDbCommand dbCommand = OpenConnection();

                    dbCommand.CommandText = query;
                    dbCommand.ExecuteNonQuery();
                    dbCommand.Dispose();

                    mutex.ReleaseMutex();
                }

                return SQLiteErrorCode.Ok;
            }

            catch (SqliteException sqle)
            {
                dbConnection.Close();
                return sqle.ErrorCode;
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        private object[] ReadQuery(string query)
        {
            try
            {
                List<object> value = new List<object>();

                if (mutex.WaitOne())
                {
                    IDataReader dataReader;
                    IDbCommand dbCommand = OpenConnection();

                    dbCommand.CommandText = query;
                    dataReader = dbCommand.ExecuteReader();

                    int i = 0;

                    while (dataReader.Read())
                    {
                        value.Add(dataReader.GetValue(i));
                    }

                    dataReader.Close();
                    dbCommand.Dispose();

                    mutex.ReleaseMutex();
                }

                return value.ToArray();
            }

            catch (SqliteException sqle)
            {
                dbConnection.Close();
                throw sqle;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        private string GetColumnName(PropertyInfo column)
        {
            var attr = column.GetCustomAttributes(typeof(TableColumn), true);

            if (attr.Length == 1)
            {
                var attribute = (TableColumn)attr[0];

                if (attribute.NameInTable == null)
                    return column.Name.ToLower();
                else
                    return attribute.NameInTable;
            }

            throw new Exception();
        }

        private string GetColumnName(TableColumn attribute, PropertyInfo column)
        {
            if (attribute.NameInTable == null)
                return column.Name.ToLower();
            else
                return attribute.NameInTable;
        }

        private string GetInsertValues(object primaryKeyValue, IList<object> values)
        {
            var valuesString = string.Format("'{0}', ", primaryKeyValue);

            for (int i = 0; i < values.Count; i++)
            {
                if (i < values.Count - 1)
                    valuesString += string.Format("'{0}', ", values[i]);
                else
                    valuesString += string.Format("'{0}'", values[i]);
            }

            return valuesString;
        }

        private string GetJoinValues(string alias, IList<PropertyInfo> columns)
        {
            var valuesString = "";

            for (int i = 0; i < columns.Count; i++)
            {
                var attr = columns[i].GetCustomAttributes(typeof(ManyToManyRelation), true);

                if (attr.Length == 0)
                {
                    if (i < columns.Count - 1)
                        valuesString += string.Format("{0}.{1}, ", alias, GetColumnName(columns[i]));
                    else
                        valuesString += string.Format("{0}.{1}", alias, GetColumnName(columns[i]));
                }
            }

            if (valuesString.Substring(valuesString.Length - 2) == ", ")
                valuesString = valuesString.Substring(0, valuesString.Length - 2);

            return valuesString;
        }

        private string GetTableName(string name)
        {
            return string.Format("{0}{1}", tablePrefix, name.ToLower());
        }

        private string GetTableName(Type model)
        {
            if (tablePrefix == "")
                return model.Name.ToLower();

            return string.Format("{0}{1}", tablePrefix, model.Name.ToLower());
        }

        private string GetTableName(ForeignKey column)
        {
            return string.Format("{0}{1}", tablePrefix, column.RelationTable.ToLower());
        }

        public SQLiteErrorCode Save(Type model, object primaryKeyValue, IList<object> values)
        {
            var query = string.Format("INSERT INTO {0} VALUES ({1})",
                GetTableName(model),
                GetInsertValues(primaryKeyValue, values)
            );

            return this.WriteQuery(query);
        }

        public SQLiteErrorCode UpdateTable(TableColumn attribute, Type model, PropertyInfo column, object value, string primaryKeyName, object primaryKeyValue)
        {
            var query = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3}='{4}'",
                GetTableName(model),
                GetColumnName(attribute, column),
                value,
                primaryKeyName.ToLower(),
                primaryKeyValue
            );

            return this.WriteQuery(query);
        }

        public object[] Get(TableColumn attribute, PropertyInfo column, Type model, string primaryKeyName, object primaryKeyValue)
        {
            var query = string.Format("SELECT `{0}` FROM {1} WHERE `{2}` = '{3}'",
                GetColumnName(attribute, column),
                GetTableName(model),
                primaryKeyName,
                primaryKeyValue
            );

            return ReadQuery(query);
        }

        public object[] Join(ManyToManyRelation manyToMany, object primaryKeyValue)
        {
            var alias = "t";

            var firstJoin = string.Format("JOIN {0} ON {1}.`{2}` = {3}.`{4}`",
                GetTableName(manyToMany.SourceTable),
                GetTableName(manyToMany.SourceTable),
                manyToMany.SourceID,
                GetTableName(manyToMany.JoinTable),
                manyToMany.JoinSourceId
            );

            var secondJoin = string.Format("JOIN {0} ON {1}.`{2}` = {3}.`{4}`",
                GetTableName(manyToMany.JoinTable),
                GetTableName(manyToMany.JoinTable),
                manyToMany.JoinTargetId,
                alias,
                manyToMany.TargetID
            );

            var where = string.Format("WHERE {0}.`{1}` = '{2}'",
                GetTableName(manyToMany.JoinTable),
                manyToMany.JoinSourceId,
                primaryKeyValue
            );

            var query = string.Format(@"SELECT {0}.{1} FROM {2} as {3} {4} {5} {6}",
                // SELECT
                alias,
                manyToMany.TargetID,
                //FROM
                GetTableName(manyToMany.TargetTable),
                // AS ALIAS
                alias,
                // FIRST JOIN
                firstJoin,
                // SECOND JOIN
                secondJoin,
                // WHERE
                where
            );

            return ReadQuery(query);
        }
    }
}