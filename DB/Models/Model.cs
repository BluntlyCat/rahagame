namespace HSA.RehaGame.DB.Models
{
    using Manager;
    using Mono.Data.Sqlite;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public abstract class Model : IModel
    {
        protected IList<PropertyInfo> tableColumns;
        protected IList<PropertyInfo> foreignKeyRelations;
        protected IList<PropertyInfo> manyToManyRelations;
        protected Type type;
        protected IDatabase database;

        private PropertyInfo primaryKey;
        private bool isInstance;

        public Model()
        {
            this.database = Database.Instance();
            this.primaryKey = GetFieldByAttribute<PrimaryKey>();
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
                try
                {
                    var attribute = GetAttribute<TableColumn>(property);
                    var attributeType = attribute.GetType();

                    if (attributeType == typeof(PrimaryKey))
                        continue;
                    else if (attributeType == typeof(ForeignKey))
                        continue;
                    else if (attributeType == typeof(ManyToManyRelation))
                        continue;

                    columns.Add(property);
                }
                catch
                {
                    continue;
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
                try
                {
                    GetAttribute<ForeignKey>(property);
                    columns.Add(property);
                }
                catch
                {
                    continue;
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
                try
                {
                    GetAttribute<ManyToManyRelation>(property);
                    columns.Add(property);
                }
                catch
                {
                    continue;
                }
            }

            return columns;
        }

        private PropertyInfo GetFieldByAttribute<T>() where T : TableColumn
        {
            PropertyInfo field = null;

            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                try
                {
                    GetAttribute<T>(property);
                    field = property;
                    break;
                }
                catch
                {
                    continue;
                }
            }

            if (field == null)
                throw new Exception(string.Format("No Field of type ({0}) found", typeof(T).Name));

            return field;
        }

        private List<object> GetValues(bool includingPrimaryKey = true)
        {
            List<object> values = new List<object>();

            if (includingPrimaryKey)
                values.Add(PrimaryKeyValue);

            foreach (var column in tableColumns)
            {
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

        private List<PropertyInfo> GetFields(bool includingPrimaryKey = true)
        {
            List<PropertyInfo> fields = new List<PropertyInfo>();

            if(includingPrimaryKey)
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

        private static T GetAttribute<T>(PropertyInfo field) where T : TableColumn
        {
            var attr = field.GetCustomAttributes(typeof(T), true);

            if (attr.Length == 1)
                return attr[0] as T;

            throw new Exception(string.Format("Field {0} is not a table column of type {1}", field.Name, typeof(T).Name));
        }

        private object GetFieldValue(PropertyInfo field)
        {
            var get = field.GetGetMethod(true);
            return get.Invoke(this, null);
        }

        private TransactionResult SaveForeignKeyRelations()
        {
            TransactionResult result = new TransactionResult(SQLiteErrorCode.Ok, null);

            foreach (var column in this.foreignKeyRelations)
            {
                var model = column.GetGetMethod().Invoke(this, null) as Model;
                result = model.Save();
            }

            return result;
        }

        private TransactionResult SaveManyToManyRelations()
        {
            TransactionResult result = new TransactionResult(SQLiteErrorCode.Ok, null);

            foreach (var column in this.manyToManyRelations)
            {
                var genericArgs = column.PropertyType.GetGenericArguments();
                var genericDict = column.GetGetMethod().Invoke(this, null);
                var concreteDict = genericDict.GetType().MakeGenericType(genericArgs) as IDictionary;

                foreach (var model in concreteDict.Values)
                {
                    result = ((Model)model).Save();
                }
            }

            return result;
        }

        public virtual TransactionResult Save()
        {
            if (primaryKey == null)
                throw new Exception(string.Format("Primary key '{0}' not set", primaryKey.Name));

            TransactionResult result = new TransactionResult(SQLiteErrorCode.Ok, null);

            if (isInstance)
            {
                foreach (var column in this.tableColumns)
                {
                    try
                    {
                        var get = column.GetGetMethod(true);
                        var value = get.Invoke(this, null);
                        var attribute = GetAttribute<TableColumn>(column);


                        if (attribute.NotNull && value == null)
                            throw new Exception(string.Format("Column '{0}' can not be null", column.Name));

                        result = database.UpdateTable(attribute, type, column, value, primaryKey.Name, GetFieldValue(primaryKey));

                        if (foreignKeyRelations.Count > 0 && result.ErrorCode == SQLiteErrorCode.Ok)
                            result = SaveForeignKeyRelations();

                        if (manyToManyRelations.Count > 0 && result.ErrorCode == SQLiteErrorCode.Ok)
                            result = SaveManyToManyRelations();
                    }

                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            else
            {
                bool includingPrimaryKey = primaryKey.Name.ToLower() != "id";
                result = database.Save(this.primaryKey.Name, type, GetFields(includingPrimaryKey), GetValues(includingPrimaryKey));

                if (result.ErrorCode == SQLiteErrorCode.Ok)
                    this.isInstance = true;
            }

            return result;
        }

        public TransactionResult AddManyToManyRelations()
        {
            TransactionResult result = new TransactionResult(SQLiteErrorCode.Ok, null);

            foreach (var column in this.manyToManyRelations)
            {
                var attribute = column.GetCustomAttributes(typeof(ManyToManyRelation), true)[0] as ManyToManyRelation;

                var genericDict = column.GetGetMethod().Invoke(this, null) as IDictionary;

                result = database.AddManyToManyRelation(attribute, this.PrimaryKeyValue, genericDict);
            }

            return result;
        }

        public TransactionResult Delete()
        {
            return database.Delete(this.type, this.primaryKey.Name, this.PrimaryKeyValue);
        }

        private object ManyToManyQuery(ManyToManyRelation attribute, PropertyInfo column)
        {
            var genericDict = typeof(Dictionary<,>);
            var genericArgs = column.PropertyType.GetGenericArguments();

            var valType = genericArgs[1];

            var values = database.Join(attribute, PrimaryKeyValue);
            var dict = genericDict.MakeGenericType(genericArgs);
            var dictInstance = Activator.CreateInstance(dict) as IDictionary;

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];
                Model model;

                if (value.ToString() != "" && value != null)
                {
                    model = database.GetCachedModel(valType, value) as Model;

                    if (model == null)
                    {
                        model = Activator.CreateInstance(valType, value) as Model;

                        if (model != null)
                        {
                            model.SetData();
                            model.SetRelations();
                            dictInstance.Add(model.PrimaryKeyValue, model);
                        }
                    }
                    else
                        dictInstance.Add(model.PrimaryKeyValue, model);
                }
            }

            return dictInstance;
        }

        private object ForeignKeyQuery(ForeignKey attribute, PropertyInfo column)
        {
            object value = null;
            var foreignKey = database.Get(attribute, column, type, GetAttribute<PrimaryKey>(primaryKey), primaryKey, PrimaryKeyValue)[0];

            if (foreignKey.ToString() != "" && foreignKey != null)
            {
                value = database.GetCachedModel(this.type, foreignKey) as IModel;

                if (value == null)
                {
                    var model = Activator.CreateInstance(column.PropertyType, foreignKey) as IModel;

                    if (model != null)
                    {
                        model.SetData();
                        model.SetRelations();
                        value = model;
                    }
                }
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
                var attribute = GetAttribute<TableColumn>(column);

                object value;

                if (attribute.GetType() == typeof(TranslationColumn))
                    ((TranslationColumn)attribute).SetColumn(column.Name, LanguageSettingManager.GetActiveLanguage());

                value = database.Get(attribute, column, type, GetAttribute<PrimaryKey>(primaryKey), primaryKey, PrimaryKeyValue)[0];

                data.Add(column, value);
            }

            return data;
        }

        protected IDictionary<PropertyInfo, object> GetRelations()
        {
            IDictionary<PropertyInfo, object> data = new Dictionary<PropertyInfo, object>();

            foreach (var foreignKeyRelation in foreignKeyRelations)
            {
                var attribute = GetAttribute<ForeignKey>(foreignKeyRelation);
                var value = ForeignKeyQuery(attribute, foreignKeyRelation);

                data.Add(foreignKeyRelation, value);
            }

            foreach (var manyToManyRelation in manyToManyRelations)
            {
                var attribute = GetAttribute<ManyToManyRelation>(manyToManyRelation);
                var value = ManyToManyQuery(attribute, manyToManyRelation);

                data.Add(manyToManyRelation, value);
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

                database.SetCachedModel(this.type, this.PrimaryKeyValue, this);

                this.isInstance = true;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual void SetRelations()
        {
            try
            {
                if (primaryKey == null)
                    throw new Exception("Primary key not set");

                var data = GetRelations();

                foreach (var d in data)
                    d.Key.GetSetMethod(true).Invoke(this, new object[] { d.Value });
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
            object[] names = null;

            foreach (var column in columns)
            {
                try
                {
                    GetAttribute<PrimaryKey>(column);
                    names = Database.Instance().All(column.Name, type.Name);
                    break;
                }
                catch
                {
                    continue;
                }
            }

            if (names == null)
                throw new Exception(string.Format("No primary key found in model of type {0}", type.Name));

            return names;
        }

        public static IDictionary<object, T> All<T>() where T : Model
        {
            IDictionary<object, T> models = new Dictionary<object, T>();

            var primaryKeys = GetPrimaryKeys(typeof(T));

            foreach (var primaryKey in primaryKeys)
            {
                var model = GetModel<T>(primaryKey);

                models.Add(model.PrimaryKeyValue, model);
            }

            return models;
        }

        public static T GetModel<T>(object primaryKey) where T : Model
        {
            IModel model = Activator.CreateInstance(typeof(T), primaryKey) as IModel;

            if (model != null)
            {
                model.SetData();
                model.SetRelations();
            }

            return model as T;
        }
    }
}
