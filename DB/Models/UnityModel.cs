namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
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

        public override void SetData()
        {
            var data = GetData();

            foreach(var d in data)
            {
                var attr = d.Key.GetCustomAttributes(typeof(TableColumn), true);

                if (attr.Length == 1)
                {
                    var attribute = attr[0];
                    var set = d.Key.GetSetMethod(true);

                    if (attribute.GetType() == typeof(ResourceColumn))
                    {
                        var resource = Resources.Load(d.Value.ToString(), d.Key.PropertyType);

                        set.Invoke(this, new object[] {  });
                    }
                    else
                    {
                        set.Invoke(this, new object[] { d.Value });
                    }
                }
            }
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
