namespace HSA.RehaGame.DB
{
    using Logging;
    using Models;
    using Mono.Data.Sqlite;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Threading;
    using UnityEngine;

    public class Database : IDatabase
    {
        protected static Logger<Database> logger = new Logger<Database>();

        private static IDatabase database;
        protected static Mutex mutex = new Mutex();

        protected string tablePrefix;
        protected IDbConnection dbConnection;
        private IDictionary<string, Dictionary<object, IModel>> cache = new Dictionary<string, Dictionary<object, IModel>>();

        protected Database(string tablePrefix)
        {
            logger.AddLogAppender<ConsoleAppender>();
#if UNITY_EDITOR
            logger.AddLogAppender<FileAppender>();
#endif

            var dbFile = "Data Source=" + Application.dataPath + "/StreamingAssets/db/rgdbe.db";

            this.tablePrefix = tablePrefix;
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

        private TransactionResult WriteQuery(string query, IList<FieldValuePair> fieldValuePairs = null)
        {
            try
            {
                if (mutex.WaitOne())
                {
                    IDbCommand dbCommand = OpenConnection();

                    dbCommand.CommandText = query;

                    if (fieldValuePairs != null)
                    {
                        foreach (var fieldValuePair in fieldValuePairs)
                        {
                            dbCommand.Parameters.Add(new SqliteParameter(
                                fieldValuePair.Name,
                                fieldValuePair.Value
                            ));
                        }
                    }

                    dbCommand.ExecuteNonQuery();

                    dbCommand.Dispose();

                    mutex.ReleaseMutex();
                }

                return new TransactionResult(SQLiteErrorCode.Ok, null);
            }

            catch (SqliteException sqle)
            {
                dbConnection.Close();
                logger.Error(sqle + " Query: " + query);
                return new TransactionResult(sqle.ErrorCode, null);
            }

            catch (Exception e)
            {
                logger.Fatal(e);
                throw e;
            }
        }

        private TransactionResult WriteQuery(string columnName, string tableName, string query, IList<FieldValuePair> fieldValuePairs = null)
        {
            try
            {
                object rowId = null;
                object primaryKeyValue = null;

                var result = this.WriteQuery(query, fieldValuePairs);

                if (result.ErrorCode == SQLiteErrorCode.Ok)
                {
                    if (mutex.WaitOne())
                    {
                        IDbCommand dbCommand = OpenConnection();

                        dbCommand.CommandText = @"select last_insert_rowid()";
                        rowId = dbCommand.ExecuteScalar();

                        dbCommand.CommandText = "SELECT " + columnName + " FROM " + tableName + " WHERE rowid=" + rowId;
                        primaryKeyValue = dbCommand.ExecuteScalar();

                        dbCommand.Dispose();

                        mutex.ReleaseMutex();
                    }

                    return new TransactionResult(SQLiteErrorCode.Ok, primaryKeyValue);
                }

                return result;
            }

            catch (SqliteException sqle)
            {
                dbConnection.Close();
                logger.Error(sqle + " Query: " + query);
                return new TransactionResult(sqle.ErrorCode, null);
            }

            catch (Exception e)
            {
                logger.Fatal(e);
                throw e;
            }
        }

        protected object[] ReadQuery(string query)
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
                logger.Error(sqle);
                throw sqle;
            }

            catch (Exception e)
            {
                logger.Fatal(e);
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

        private string GetInsertValues(IList<PropertyInfo> fields)
        {
            var valuesString = "VALUES(";

            for (int i = 0; i < fields.Count; i++)
            {
                var attribute = fields[i].GetCustomAttributes(typeof(TableColumn), true)[0] as TableColumn;
                var name = attribute.NameInTable == null ? fields[i].Name.ToLower() : attribute.NameInTable;

                if (i < fields.Count - 1)
                    valuesString += string.Format("@{0}, ", name);
                else
                    valuesString += string.Format("@{0})", name);
            }

            return valuesString;
        }

        private string GetInsertColumns(IList<PropertyInfo> fields)
        {
            var valuesString = "(";

            for (int i = 0; i < fields.Count; i++)
            {
                var attribute = fields[i].GetCustomAttributes(typeof(TableColumn), true)[0] as TableColumn;
                var name = attribute.NameInTable == null ? fields[i].Name.ToLower() : attribute.NameInTable;

                if (i < fields.Count - 1)
                    valuesString += string.Format("{0}, ", name);
                else
                    valuesString += string.Format("{0})", name);
            }

            return valuesString;
        }

        private IList<FieldValuePair> GetFieldValues(IList<PropertyInfo> fields, IList<object> values)
        {
            List<FieldValuePair> fieldValuePairs = new List<FieldValuePair>();

            if (fields.Count != values.Count)
                throw new Exception("Amount of fields and values has to be equal");

            for (int i = 0; i < fields.Count; i++)
            {
                var attribute = fields[i].GetCustomAttributes(typeof(TableColumn), true)[0] as TableColumn;
                var name = attribute.NameInTable == null ? fields[i].Name.ToLower() : attribute.NameInTable;

                fieldValuePairs.Add(new FieldValuePair(name, values[i]));
            }

            return fieldValuePairs;
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

        public void SetCachedModel(Type modelType, object primaryKeyValue, IModel model)
        {
            var tableName = GetTableName(modelType);

            if (this.cache.ContainsKey(tableName) == false)
                this.cache.Add(tableName, new Dictionary<object, IModel>());

            if (this.cache[tableName].ContainsKey(primaryKeyValue))
                this.cache[tableName][primaryKeyValue] = model;

            else
                this.cache[tableName].Add(primaryKeyValue, model);
        }

        public IModel GetCachedModel(Type modelType, object primaryKeyValue)
        {
            var tableName = GetTableName(modelType);

            if (this.cache.ContainsKey(tableName) && this.cache[tableName].ContainsKey(primaryKeyValue))
                return this.cache[tableName][primaryKeyValue];

            return null;
        }

        public TransactionResult Save(string primaryKeyName, Type model, List<PropertyInfo> fields, List<object> values)
        {
            var tableName = GetTableName(model);

            var query = string.Format("INSERT INTO {0} {1} {2}",
                tableName,
                GetInsertColumns(fields),
                GetInsertValues(fields)
            );

            return this.WriteQuery(primaryKeyName.ToLower(), tableName, query, GetFieldValues(fields, values));
        }

        public TransactionResult AddManyToManyRelations(ManyToManyRelation attribute, object sourceId, IDictionary models)
        {
            TransactionResult result = new TransactionResult(SQLiteErrorCode.Ok, null);

            if (models != null)
            {
                foreach (object targetId in models.Keys)
                {
                    var query = string.Format("INSERT INTO {0} ({1}, {2}) VALUES('{3}', '{4}')",
                        attribute.JoinTable,
                        attribute.JoinSourceId,
                        attribute.JoinTargetId,
                        sourceId,
                        targetId
                    );

                    result = this.WriteQuery(query);
                }
            }

            return result;
        }

        public TransactionResult AddManyToManyRelation(ManyToManyRelation attribute, object sourceId, Model model)
        {
            TransactionResult result = new TransactionResult(SQLiteErrorCode.Ok, null);

            if (model != null)
            {
                var query = string.Format("INSERT INTO {0} ({1}, {2}) VALUES('{3}', '{4}')",
                        attribute.JoinTable,
                        attribute.JoinSourceId,
                        attribute.JoinTargetId,
                        sourceId,
                        model.PrimaryKeyValue
                    );

                result = this.WriteQuery(query);
            }

            return result;
        }

        public TransactionResult Delete(Type model, string primaryKeyName, object primaryKeyValue)
        {
            var query = string.Format("DELETE FROM {0} WHERE {1} = '{2}'",
                GetTableName(model),
                primaryKeyName.ToLower(),
                primaryKeyValue
            );

            return this.WriteQuery(query);
        }

        public TransactionResult UpdateTable(TableColumn attribute, Type model, PropertyInfo column, object value, string primaryKeyName, object primaryKeyValue)
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

        public object[] All(string primaryKeyField, string modelName)
        {
            var tableName = GetTableName(modelName);
            var query = string.Format("SELECT `{0}` FROM {1}", primaryKeyField.ToLower(), tableName);

            var data = ReadQuery(query);

            return data;
        }

        public object[] Get(TableColumn attribute, PropertyInfo column, Type model, PrimaryKey pkAttribute, PropertyInfo primaryKey, object primaryKeyValue)
        {
            var tableName = GetTableName(model);
            var query = string.Format("SELECT `{0}` FROM {1} WHERE `{2}` = '{3}'",
                GetColumnName(attribute, column),
                tableName,
                GetColumnName(pkAttribute, primaryKey),
                primaryKeyValue
            );

            var data = ReadQuery(query);

            return data;
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