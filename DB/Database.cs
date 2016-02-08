namespace HSA.RehaGame.DB
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using Models;
    using Mono.Data.Sqlite;
    using UnityEngine;

    public class Database : MonoBehaviour, IDatabase
    {
        public string prefix;

        private IList<Type> knownModels = new List<Type>();
        private IDbConnection dbConnection;

        void Awake()
        {
            var dbFile = "Data Source=" + Application.dataPath + "/StreamingAssets/db/rgdbe.db";
            dbConnection = new SqliteConnection(dbFile);
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
                IDbCommand dbCommand = OpenConnection();

                dbCommand.CommandText = query;
                dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();

                return SQLiteErrorCode.Ok;
            }

            catch (SqliteException sqle)
            {
                return sqle.ErrorCode;
            }

            catch (Exception e)
            {
                throw e;
            }

            finally
            {
                dbConnection.Close();
            }
        }
        private object[] ReadQuery(string query)
        {
            try
            {
                object[] value;
                IDataReader dataReader;
                IDbCommand dbCommand = OpenConnection();

                dbCommand.CommandText = query;
                dataReader = dbCommand.ExecuteReader();
                value = new object[dataReader.FieldCount];

                while (dataReader.Read())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        value[i] = dataReader.GetValue(i);
                    }
                }

                dataReader.Close();
                dbCommand.Dispose();

                return value;
            }

            finally
            {
                dbConnection.Close();
            }
        }

        private object[] QueryValue(string query)
        {
            return ReadQuery(query);
        }

        private string GetColumnName(TableColumn attribute, PropertyInfo column)
        {
            if(attribute.NameInTable == null)
                return column.Name.ToLower();
            else
                return attribute.NameInTable;
        }

        private string GetInsertValues(object primaryKeyValue, IList<object> values)
        {
            var valuesString = string.Format("'{0}', ", primaryKeyValue);

            for(int i = 0; i < values.Count; i++)
            {
                if (i < values.Count - 1)
                    valuesString += string.Format("'{0}', ", values[i]);
                else
                    valuesString += string.Format("'{0}'", values[i]);
            }

            return valuesString;
        }

        private string GetTableName(Type model)
        {
            return string.Format("{0}{1}", prefix, model.Name.ToLower());
        }

        private string GetTableName(ForeignKey column)
        {
            return string.Format("{0}{1}", prefix, column.RelationTable.ToLower());
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
            var query = string.Format("SELECT {0} FROM {1} WHERE {2} = '{3}'",
                GetColumnName(attribute, column),
                GetTableName(model),
                primaryKeyName,
                primaryKeyValue
            );

            return QueryValue(query);
        }

        public object[] Get(ManyToManyRelation attribute, PropertyInfo column, object primaryKeyValue)
        {
            var query = string.Format("SELECT {0} FROM {1} WHERE {2} = '{3}'",
                attribute.ToID,
                GetTableName(attribute),
                attribute.FromID,
                primaryKeyValue
            );

            return QueryValue(query);
        }
    }
}