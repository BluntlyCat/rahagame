namespace HSA.RehaGame.DB.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Mono.Data.Sqlite;

    public abstract class Model : IModel
    {
        protected IList<PropertyInfo> tableColumns;
        protected IList<PropertyInfo> foreignKeyRelations;
        protected IList<PropertyInfo> manyToManyRelations;

        private PropertyInfo primaryKey;
        private IDatabase database;
        private Type type;
        private bool isInstance;

        public Model()
        {
            this.database = Database.Instance();
            this.primaryKey = GetFieldOfType(typeof(PrimaryKey));
            this.type = this.GetType();
            this.tableColumns = GetTableColumns(type);
            this.foreignKeyRelations = GetForeignKeyRelations(type);
            this.manyToManyRelations = GetManyToManyRelations(type);

            if (tableColumns.Count == 0 && foreignKeyRelations.Count == 0 && manyToManyRelations.Count == 0)
                throw new Exception("No columns found");

            this.isInstance = false;
        }

        public bool IsInstance
        {
            get
            {
                return isInstance;
            }
        }

        public object PrimaryKeyValue
        {
            get
            {
                return primaryKey.GetGetMethod().Invoke(this, null);
            }
        }

        private IList<PropertyInfo> GetTableColumns(Type model)
        {
            IList<PropertyInfo> columns = new List<PropertyInfo>();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (PropertyInfo property in this.GetType().GetProperties(flags))
            {
                var attr = property.GetCustomAttributes(typeof(TableColumn), true);

                if (attr.Length == 1)
                {
                    var attributeType = attr[0].GetType();

                    if (attributeType == typeof(PrimaryKey))
                        continue;
                    else if (attributeType == typeof(ForeignKey))
                        continue;
                    else if (attributeType == typeof(ManyToManyRelation))
                        continue;

                    columns.Add(property);
                }
            }

            return columns;
        }

        private IList<PropertyInfo> GetForeignKeyRelations(Type model)
        {
            IList<PropertyInfo> columns = new List<PropertyInfo>();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (PropertyInfo property in this.GetType().GetProperties(flags))
            {
                var attr = property.GetCustomAttributes(typeof(ForeignKey), true);

                if (attr.Length == 1)
                {
                    columns.Add(property);
                }
            }

            return columns;
        }
        private IList<PropertyInfo> GetManyToManyRelations(Type model)
        {
            IList<PropertyInfo> columns = new List<PropertyInfo>();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (PropertyInfo property in this.GetType().GetProperties(flags))
            {
                var attr = property.GetCustomAttributes(typeof(ManyToManyRelation), true);

                if (attr.Length == 1)
                {
                    columns.Add(property);
                }
            }

            return columns;
        }

        private PropertyInfo GetFieldOfType(Type type)
        {
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                var attr = property.GetCustomAttributes(type, true);

                if (attr.Length == 1)
                {
                    return property;
                }
            }

            if (type.Equals(typeof(PrimaryKey)))
                throw new Exception(string.Format("No Field of type ({0}) found", type.Name));

            return null;
        }

        private List<object> GetValues()
        {
            List<object> values = new List<object>();

            values.Add(PrimaryKeyValue);

            foreach (var column in tableColumns)
            {
                var attributes = column.GetCustomAttributes(typeof(TableColumn), true)[0];

                if (column == primaryKey)
                    continue;

                var get = column.GetGetMethod(true);
                values.Add(get.Invoke(this, null));
            }

            foreach (var column in foreignKeyRelations)
            {
                if (column == primaryKey)
                    continue;

                var get = column.GetGetMethod(true);
                var model = get.Invoke(this, null) as Model;
                values.Add(model.PrimaryKeyValue);
            }

            return values;
        }

        private List<PropertyInfo> GetFields()
        {
            List<PropertyInfo> fields = new List<PropertyInfo>();

            fields.Add(primaryKey);

            foreach (var column in tableColumns)
            {
                var attributes = column.GetCustomAttributes(typeof(TableColumn), true);

                if (attributes.Length == 0)
                    continue;

                fields.Add(column);
            }

            foreach (var column in foreignKeyRelations)
            {
                var attributes = column.GetCustomAttributes(typeof(TableColumn), true);

                if (attributes.Length == 0)
                    continue;

                fields.Add(column);
            }

            return fields;
        }

        private string GetFieldColumnName(PropertyInfo field)
        {
            var attr = field.GetCustomAttributes(typeof(TableColumn), true);

            if (attr.Length == 1)
            {
                var attribute = (TableColumn)attr[0];

                if (attribute.NameInTable == null)
                    return field.Name.ToLower();

                return attribute.NameInTable;
            }

            throw new Exception(string.Format("Field ({0}) is not a table column", field.Name));
        }

        private object GetFieldValue(PropertyInfo field)
        {
            var get = field.GetGetMethod(true);
            return get.Invoke(this, null);
        }

        private SQLiteErrorCode SaveForeignKeyRelations()
        {
            SQLiteErrorCode errorCode = SQLiteErrorCode.Ok;

            foreach (var column in this.foreignKeyRelations)
            {
                var model = column.GetGetMethod().Invoke(this, null) as Model;
                errorCode = model.Save();
            }

            return errorCode;
        }

        private SQLiteErrorCode SaveManyToManyRelations()
        {
            SQLiteErrorCode errorCode = SQLiteErrorCode.Ok;

            foreach (var column in this.manyToManyRelations)
            {
                var genericArgs = column.PropertyType.GetGenericArguments();
                var genericDict = column.GetGetMethod().Invoke(this, null);
                var concreteDict = genericDict.GetType().MakeGenericType(genericArgs) as IDictionary;

                foreach (var model in concreteDict.Values)
                {
                    errorCode = ((Model)model).Save();
                }
            }

            return errorCode;
        }

        public SQLiteErrorCode Save()
        {
            if (primaryKey == null)
                throw new Exception(string.Format("Primary key '{0}' not set", primaryKey.Name));

            SQLiteErrorCode errorCode = SQLiteErrorCode.Ok;

            if (isInstance)
            {
                foreach (var column in this.tableColumns)
                {
                    var attr = column.GetCustomAttributes(typeof(TableColumn), true);

                    if (attr.Length == 1)
                    {
                        try
                        {
                            var get = column.GetGetMethod(true);
                            var value = get.Invoke(this, null);
                            var attribute = ((TableColumn)attr[0]);


                            if (attribute.NotNull && value == null)
                                throw new Exception(string.Format("Column '{0}' can not be null", column.Name));

                            errorCode = database.UpdateTable(attribute, type, column, value, primaryKey.Name, GetFieldValue(primaryKey));

                            if (foreignKeyRelations.Count > 0 && errorCode == SQLiteErrorCode.Ok)
                                errorCode = SaveForeignKeyRelations();

                            if (manyToManyRelations.Count > 0 && errorCode == SQLiteErrorCode.Ok)
                                errorCode = SaveManyToManyRelations();
                        }

                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
            }

            else
            {
                errorCode = database.Save(type, GetFields(), GetValues());

                if (errorCode == SQLiteErrorCode.Ok)
                    this.isInstance = true;
            }

            return errorCode;
        }

        public SQLiteErrorCode AddManyToManyRelations()
        {
            SQLiteErrorCode errorCode = SQLiteErrorCode.Ok;

            foreach (var column in this.manyToManyRelations)
            {
                var attribute = column.GetCustomAttributes(typeof(ManyToManyRelation), true)[0] as ManyToManyRelation;

                var genericArgs = column.PropertyType.GetGenericArguments();
                var genericDict = column.GetGetMethod().Invoke(this, null) as IDictionary;

                errorCode = database.AddManyToManyRelation(attribute, this.PrimaryKeyValue, genericDict);
            }

            return errorCode;
        }

        public SQLiteErrorCode Delete()
        {
            return database.Delete(this.type, this.primaryKey.Name, this.PrimaryKeyValue);
        }

        private object ManyToManyQuery(TableColumn attribute, PropertyInfo column)
        {
            var manyToMany = ((ManyToManyRelation)attribute);
            var genericDict = typeof(Dictionary<,>);
            var genericArgs = column.PropertyType.GetGenericArguments();

            var keyType = genericArgs[0];
            var valType = genericArgs[1];

            var values = database.Join(manyToMany, PrimaryKeyValue);
            var dict = genericDict.MakeGenericType(genericArgs);
            var dictInstance = Activator.CreateInstance(dict) as IDictionary;

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];

                if (value.ToString() != "" && value != null)
                {
                    var model = Activator.CreateInstance(valType, value) as Model;

                    if (model != null)
                    {
                        model.SetData();
                        dictInstance.Add(model.PrimaryKeyValue, model);
                    }
                }
            }

            return dictInstance;
        }

        private object ForeignKeyQuery(TableColumn attribute, PropertyInfo column)
        {
            var value = database.Get(attribute, column, type, primaryKey.Name, GetFieldValue(primaryKey))[0];

            if (value.ToString() != "" && value != null)
            {
                var model = Activator.CreateInstance(column.PropertyType, value) as IModel;

                if (model != null)
                    value = model;
            }
            else
                value = null;

            return value;
        }

        protected IDictionary<PropertyInfo, object> GetData()
        {
            IDictionary<PropertyInfo, object> data = new Dictionary<PropertyInfo, object>();

            foreach (var column in this.tableColumns)
            {
                var attr = column.GetCustomAttributes(typeof(TableColumn), true);
                var attribute = ((TableColumn)attr[0]);

                if (attr.Length == 1)
                {
                    object value;

                    if (attribute.GetType() == typeof(TranslationColumn))
                        ((TranslationColumn)attribute).SetColumn(column.Name, "de_de"); //ToDo: Sprache aus Settings dynamisch setzen

                    value = database.Get(attribute, column, type, primaryKey.Name, PrimaryKeyValue)[0];

                    data.Add(column, value);
                }
            }

            foreach (var foreignKeyRelation in foreignKeyRelations)
            {
                var attr = foreignKeyRelation.GetCustomAttributes(typeof(TableColumn), true);
                var attribute = ((TableColumn)attr[0]);

                if (attr.Length == 1)
                {
                    var value = ForeignKeyQuery(attribute, foreignKeyRelation);
                    data.Add(foreignKeyRelation, value);
                }
            }

            foreach (var manyToManyRelation in manyToManyRelations)
            {
                var attr = manyToManyRelation.GetCustomAttributes(typeof(TableColumn), true);
                var attribute = ((TableColumn)attr[0]);

                if (attr.Length == 1)
                {
                    var value = ManyToManyQuery(attribute, manyToManyRelation);
                    data.Add(manyToManyRelation, value);
                }
            }

            return data;
        }

        public virtual void SetData()
        {
            try
            {
                if (primaryKey == null)
                    throw new Exception("Primary key not set");

                var data = GetData();

                foreach (var d in data)
                    d.Key.GetSetMethod(true).Invoke(this, new object[] { d.Value });

                this.isInstance = true;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} = {2}", this.GetType().Name, primaryKey.Name, primaryKey.GetGetMethod(true).Invoke(this, null));
        }

        private static object[] GetPrimaryKeys(Type type)
        {
            var columns = type.GetProperties();

            foreach (var column in columns)
            {
                var attr = column.GetCustomAttributes(typeof(PrimaryKey), true);

                if (attr.Length == 1)
                {
                    return Database.Instance().All(column.Name, type.Name);
                }
            }

            throw new Exception(string.Format("No primary key found in model of type {0}", type.Name));
        }

        public static IDictionary<object, T> All<T>() where T : Model
        {
            Type type = typeof(T);
            IDictionary<object, T> models = new Dictionary<object, T>();

            var primaryKeys = GetPrimaryKeys(typeof(T));

            foreach (var primaryKey in primaryKeys)
            {
                var model = GetModel<T>(primaryKey);

                model.SetData();
                models.Add(model.PrimaryKeyValue, model);
            }

            return models;
        }

        public static T GetModel<T>(object primaryKey) where T : Model
        {
            IModel model = Activator.CreateInstance(typeof(T), primaryKey) as IModel;

            if (model != null)
                model.SetData();

            return model as T;
        }
    }
}
