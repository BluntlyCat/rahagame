namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
    using Kinect = Windows.Kinect;

    public class Joint : Model
    {
        private string name;
        private long value;

        private bool xyAxis;
        private bool yzAxis;
        private bool zxAxis;

        private long xyMinValue;
        private long xyMaxValue;

        private long yzMinValue;
        private long yzMaxValue;

        private long zxMinValue;
        private long zxMaxValue;

        private string translation;

        private Joint parent;
        private Dictionary<string, Joint> children;

        public Joint(string name)
        {
            this.name = name;
        }

        [PrimaryKey]
        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                this.name = value;
            }
        }

        [TableColumn]
        private long Value
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

        public Kinect.JointType Type
        {
            get
            {
                return (Kinect.JointType)this.value;
            }

            set
            {
                this.value = (long)value;
            }
        }

        [TableColumn("xyAxis")]
        public bool XYAxis
        {
            get
            {
                return xyAxis;
            }
            set
            {
                xyAxis = value;
            }
        }

        [TableColumn("yzAxis")]
        public bool YZAxis
        {
            get
            {
                return yzAxis;
            }
            set
            {
                yzAxis = value;
            }
        }

        [TableColumn("zxAxis")]
        public bool ZXAxis
        {
            get
            {
                return zxAxis;
            }
            set
            {
                zxAxis = value;
            }
        }

        [TableColumn]
        public long XYMinValue
        {
            get
            {
                return this.xyMinValue;
            }

            set
            {
                this.xyMinValue = value;
            }
        }

        [TableColumn]
        public long XYMaxValue
        {
            get
            {
                return this.xyMaxValue;
            }

            set
            {
                this.xyMaxValue = value;
            }
        }

        [TableColumn]
        public long YZMinValue
        {
            get
            {
                return this.yzMinValue;
            }

            set
            {
                this.yzMinValue = value;
            }
        }

        [TableColumn]
        public long YZMaxValue
        {
            get
            {
                return this.yzMaxValue;
            }

            set
            {
                this.yzMaxValue = value;
            }
        }

        [TableColumn]
        public long ZXMinValue
        {
            get
            {
                return this.zxMinValue;
            }

            set
            {
                this.zxMinValue = value;
            }
        }

        [TableColumn]
        public long ZXMaxValue
        {
            get
            {
                return this.zxMaxValue;
            }

            set
            {
                this.zxMaxValue = value;
            }
        }

        [TranslationColumn]
        public string Translation
        {
            get
            {
                return this.translation;
            }

            set
            {
                this.translation = value;
            }
        }

        [ForeignKey("joint", "parent_id", true)]
        public Joint Parent
        {
            get
            {
                if (parent == null)
                    return null;

                else if(!parent.IsInstance)
                    parent.SetData();

                return this.parent;
            }

            set
            {
                this.parent = value;
            }
        }

        [ManyToManyRelation(
            "name",
            "joint",
            "from_joint_id",
            "joint_children",
            "to_joint_id",
            "joint",
            "name"
        )]
        public Dictionary<string, Joint> Children
        {
            get
            {
                return this.children;
            }

            set
            {
                this.children = value;
            }
        }
    }
}
