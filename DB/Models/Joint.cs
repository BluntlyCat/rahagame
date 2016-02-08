namespace HSA.RehaGame.DB.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using DB;

    public class Joint : Model
    {
        private string name;

        private long kinectValue;

        private bool x_axis;
        private bool y_axis;
        private bool z_axis;

        private long x_axis_min_value;
        private long x_axis_max_value;

        private long y_axis_min_value;
        private long y_axis_max_value;

        private long z_axis_min_value;
        private long z_axis_max_value;

        private string translation;

        private Joint parent;
        private Dictionary<string, Joint> children;

        public Joint(string name, IDatabase database) : base(database)
        {
            this.name = name;
        }

        public Joint(IDatabase database) : base(database)
        {
        }

        [TableColumn]
        [PrimaryKey]
        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                this.name = value;
            }
        }

        [TableColumn("value")]
        private long KinectValue
        {
            get
            {
                return kinectValue;
            }
            set
            {
                kinectValue = value;
            }
        }

        // ToDo public Kinect.JointType get...

        [TableColumn]
        public bool X_Axis
        {
            get
            {
                return x_axis;
            }
            set
            {
                x_axis = value;
            }
        }

        [TableColumn]
        public bool Y_Axis
        {
            get
            {
                return y_axis;
            }
            set
            {
                y_axis = value;
            }
        }

        [TableColumn]
        public bool Z_Axis
        {
            get
            {
                return z_axis;
            }
            set
            {
                z_axis = value;
            }
        }

        [TableColumn]
        public long X_Axis_Min_Value
        {
            get
            {
                return this.x_axis_min_value;
            }

            set
            {
                this.x_axis_min_value = value;
            }
        }

        [TableColumn]
        public long X_Axis_Max_Value
        {
            get
            {
                return this.x_axis_max_value;
            }

            set
            {
                this.x_axis_max_value = value;
            }
        }

        [TableColumn]
        public long Y_Axis_Min_Value
        {
            get
            {
                return this.y_axis_min_value;
            }

            set
            {
                this.y_axis_min_value = value;
            }
        }

        [TableColumn]
        public long Y_Axis_Max_Value
        {
            get
            {
                return this.y_axis_max_value;
            }

            set
            {
                this.y_axis_max_value = value;
            }
        }

        [TableColumn]
        public long Z_Axis_Min_Value
        {
            get
            {
                return this.z_axis_min_value;
            }

            set
            {
                this.z_axis_min_value = value;
            }
        }

        [TableColumn]
        public long Z_Axis_Max_Value
        {
            get
            {
                return this.z_axis_max_value;
            }

            set
            {
                this.z_axis_max_value = value;
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
                    parent.Get();

                return this.parent;
            }

            set
            {
                this.parent = value;
            }
        }

        [ManyToManyRelation("from_joint_id", "joint_children", "to_joint_id", true)]
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
