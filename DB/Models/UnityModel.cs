namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class UnityModel : Model
    {
        private string unityObjectName;
        private string unityTagName;

        public UnityModel(string unityObjectName)
        {
            this.unityObjectName = unityObjectName;
        }

        public override void SetData()
        {
            var data = GetData();

            foreach(var d in data)
            {
                var attr = d.Key.GetCustomAttributes(typeof(TableColumn), true);

                if (attr.Length == 1)
                {
                    var set = d.Key.GetSetMethod(true);

                    if (d.Key.GetCustomAttributes(typeof(Resource), true).Length == 1)
                    {
                        Object resource;

                        if (d.Value == null)
                            resource = null;
                        else
                        {
                            string relativePath = d.Value.ToString();
                            string resourcePath = relativePath.Replace("Assets/Resources/", "");
                            string resourceName = resourcePath.Substring(0, resourcePath.Length - 4);

                            resource = Resources.Load(resourceName, d.Key.PropertyType);
                        }

                        set.Invoke(this, new object[] { resource });
                    }
                    else
                    {
                        set.Invoke(this, new object[] { d.Value });
                    }
                }
            }

            database.SetCachedModel(this.type, this.PrimaryKeyValue, this);
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
