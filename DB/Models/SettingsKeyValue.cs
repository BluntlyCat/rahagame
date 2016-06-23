namespace HSA.RehaGame.DB.Models
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class SettingsKeyValue : Model
    {
        private string key;
        private object value;
        private string stringType;

        private Dictionary<string, Type> typeMapping = new Dictionary<string, Type>()
        {
            { "str", typeof(string) },
            { "int", typeof(int) },
            { "bool", typeof(bool) },
            { "list", typeof(List<object>) },
            { "NoneType", null },
        };

        private Dictionary<Type, MethodInfo> methodMapping;

        public SettingsKeyValue(string key)
        {
            this.key = key;
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            methodMapping = new Dictionary<Type, MethodInfo>()
            {
                { typeof(string), this.GetType().GetMethod("GetString", flags) },
                { typeof(int), this.GetType().GetMethod("GetInt", flags) },
                { typeof(bool), this.GetType().GetMethod("GetBool", flags) },
                { typeof(List<object>), this.GetType().GetMethod("GetList", flags) },
            };
        }

        private string GetType(Type t)
        {
            foreach (var typeMap in typeMapping)
            {
                if (typeMap.Value == t)
                    return typeMap.Key;
            }

            return "NoneType";
        }

        [PrimaryKey]
        public string Key
        {
            get
            {
                return this.key;
            }

            private set
            {
                this.key = value;
            }
        }

        [TableColumn]
        public object Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }

        [TableColumn("dataType")]
        private string StringType
        {
            get
            {
                return this.stringType;
            }

            set
            {
                this.stringType = value;
            }
        }

        public Type Type
        {
            get
            {
                return this.typeMapping[this.stringType];
            }

            set
            {
                this.stringType = GetType(value);
            }
        }

        public object GetValue()
        {
            var converter = this.ConvertValue();
            var value = converter.Invoke(this, new object[] { this.Value });

            return value;
        }

        public T GetValue<T>()
        {
            return (T)this.ConvertValue().Invoke(this, new object[] { this.value });
        }

        public void SetValue<T>(T value)
        {
            this.value = value.ToString().ToLower();
            this.Save();
        }

        private MethodInfo ConvertValue()
        {
            if (typeMapping.ContainsKey(this.stringType))
            {
                Type type = typeMapping[this.stringType];
                MethodInfo method = this.methodMapping[type];

                return method;
            }

            throw new KeyNotFoundException(string.Format("There is no key of type {0}", this.stringType));
        }

        private bool GetBool(object value)
        {
            return bool.Parse(value.ToString());
        }

        private int GetInt(object value)
        {
            return int.Parse(value.ToString());
        }

        private string GetString(object value)
        {
            return value.ToString();
        }

        private List<object> GetList(object value)
        {
            return new List<object>(value.ToString().Split(','));
        }
    }
}
