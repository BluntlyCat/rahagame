namespace HSA.RehaGame.DB.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Mono.Data.Sqlite;

    public abstract class Model : IModel
    {
        private IList<PropertyInfo> columns;
        private PropertyInfo primaryKey;
        private IDatabase database;
        private Type type;
        private bool isInstance;

        public Model(IDatabase database)
        {
            this.database = database;
            this.primaryKey = GetPrimaryKey();
            this.columns = GetColumns();
            this.type = this.GetType();
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

        private IList<PropertyInfo> GetColumns()
        {
            IList<PropertyInfo> columns = new List<PropertyInfo>();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (PropertyInfo property in this.GetType().GetProperties(flags))
            {
                if (property != primaryKey)
                {
                    var attr = property.GetCustomAttributes(typeof(TableColumn), true);

                    if (attr.Length == 1)
                        columns.Add(property);
                }
            }

            if (columns.Count == 0)
                throw new Exception("No columns found");

            return columns;
        }

        private PropertyInfo GetPrimaryKey()
        {
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                var attr = property.GetCustomAttributes(typeof(PrimaryKey), true);

                if (attr.Length == 1)
                {
                    return property;
                }
            }

            throw new Exception("No primary key found");
        }

        private IList<object> GetValues()
        {
            IList<object> values = new List<object>();

            foreach (var column in columns)
            {
                var get = column.GetGetMethod(true);
                values.Add(get.Invoke(this, null));
            }

            return values;
        }
        private object GetPrimaryKeyValue()
        {
            var get = primaryKey.GetGetMethod(true);
            return get.Invoke(this, null);
        }

        public SQLiteErrorCode Save()
        {
            if (primaryKey == null)
                throw new Exception(string.Format("Primary key '{0}' not set", primaryKey.Name));

            foreach (var column in this.columns)
            {
                var attr = column.GetCustomAttributes(typeof(TableColumn), true);

                if (attr.Length == 1)
                {
                    try
                    {
                        SQLiteErrorCode errorCode;
                        var get = column.GetGetMethod(true);
                        var value = get.Invoke(this, null);
                        var attribute = ((TableColumn)attr[0]);


                        if (attribute.NotNull && value == null)
                            throw new Exception(string.Format("Column '{0}' can not be null", column.Name));

                        if (this.isInstance)
                            errorCode = database.UpdateTable(attribute, type, column, value, primaryKey.Name, GetPrimaryKeyValue());
                        else
                            errorCode = database.Save(type, GetPrimaryKeyValue(), GetValues());

                        if(errorCode == SQLiteErrorCode.Ok)
                            this.isInstance = true;
                        else
                            return errorCode;
                    }

                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            return SQLiteErrorCode.Ok;
        }

        private object ManyToManyQuery(TableColumn attribute, PropertyInfo column)
        {
            var manyToMany = ((ManyToManyRelation)attribute);
            var values = database.Get(manyToMany, column, GetPrimaryKeyValue());
            var genericDict = typeof(Dictionary<,>);
            var genericArgs = column.PropertyType.GetGenericArguments();
            var dict = genericDict.MakeGenericType(genericArgs);
            var dictInstance = Activator.CreateInstance(dict) as IDictionary;

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];

                if (value.ToString() != "" && value != null)
                {
                    var model = Activator.CreateInstance(genericArgs[1], value, database) as Model;

                    if (model != null)
                        dictInstance.Add(model.PrimaryKeyValue, model);
                }
            }

            return dictInstance;
        }

        private object ForeignKeyQuery(TableColumn attribute, PropertyInfo column)
        {
            var value = database.Get(attribute, column, type, primaryKey.Name, GetPrimaryKeyValue())[0];

            if (value.ToString() != "" && value != null)
            {
                var model = Activator.CreateInstance(column.PropertyType, value, database) as IModel;

                if (model != null)
                    value = model;
            }
            else
                value = null;

            return value;
        }

        public void Get()
        {
            try
            {
                if (primaryKey == null)
                    throw new Exception(string.Format("Primary key '{0}' not set", primaryKey.Name));

                foreach (var column in this.columns)
                {
                    var attr = column.GetCustomAttributes(typeof(TableColumn), true);

                    if (attr.Length == 1)
                    {
                        object value;
                        var attribute = ((TableColumn)attr[0]);
                        var set = column.GetSetMethod(true);

                        if (attribute.GetType() == typeof(TranslationColumn))
                            ((TranslationColumn)attribute).SetColumn(column.Name, "de_de"); //ToDo: Sprache aus Settings dynamisch setzen

                        if (attribute.GetType() == typeof(ManyToManyRelation))
                            value = ManyToManyQuery(attribute, column);

                        else if (attribute.GetType() == typeof(ForeignKey))
                            value = ForeignKeyQuery(attribute, column);

                        else
                            value = database.Get(attribute, column, type, primaryKey.Name, GetPrimaryKeyValue())[0];

                        set.Invoke(this, new object[] { value });
                    }
                }

                this.isInstance = true;
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public override string ToString()
        {
            string values = string.Format("{0}: {1} ({2})\n", primaryKey.Name, primaryKey.GetGetMethod(true).Invoke(this, null), primaryKey.PropertyType.Name);

            foreach(var column in columns)
            {
                var value = column.GetGetMethod(true).Invoke(this, null);

                if(value != null)
                    values += string.Format("{0}: {1} ({2})\n", column.Name, value, column.PropertyType.Name);
            }

            return values;
        }
    }
}
