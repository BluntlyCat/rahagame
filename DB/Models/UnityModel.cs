namespace HSA.RehaGame.DB.Models
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;

    public abstract class UnityModel : Model
    {
        private string unityObjectName;
        private string unityTagName;
        private IDictionary<string, UnityEngine.Object> resources = new Dictionary<string, UnityEngine.Object>();

        public UnityModel(string unityObjectName)
        {
            this.unityObjectName = unityObjectName;
        }

        public override void Get()
        {
            base.Get();
            LoadResources(this.GetResourceFields());
        }

        private IList<PropertyInfo> GetResourceFields()
        {
            IList< PropertyInfo> resourceFields = new List<PropertyInfo>();

            foreach(var column in columns)
            {
                var attr = column.GetCustomAttributes(typeof(ResourceColumn), true);

                if (attr.Length == 1)
                    resourceFields.Add(column);
            }

            return resourceFields;
        }

        private void LoadResources(IList<PropertyInfo> resourceFields)
        {
            foreach(var resourceField in resourceFields)
            {
                var fullPath = resourceField.GetGetMethod().Invoke(this, null) as string;
                var relativePath = fullPath.Replace("Assets/Resources/", "");
                var filePath = relativePath.Substring(0, relativePath.Length - 4);

                resources.Add(resourceField.Name, Resources.Load(filePath));
            }
        }

        public T GetResource<T>(string name) where T : UnityEngine.Object
        {
            if (resources.ContainsKey(name))
                return resources[name] as T;

            throw new Exception(string.Format("There is no resource named {0}", name));
        }

        [PrimaryKey]
        public string UnityObjectName
        {
            get
            {
                return this.unityObjectName;
            }

            set
            {
                this.unityObjectName = value;
            }
        }

        [TableColumn(notNull: false, unique: true)]
        public string UnityTagName
        {
            get
            {
                return this.unityTagName;
            }

            set
            {
                this.unityTagName = value;
            }
        }
    }
}
