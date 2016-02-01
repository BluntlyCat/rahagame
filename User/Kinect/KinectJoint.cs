namespace HSA.RehaGame.User.Kinect
{
    using System.Collections.Generic;
    using System.Linq;
    using DB;
    using User;
    using Windows.Kinect;

    public abstract class KinectJoint : DBObject
    {
        private PatientJoint parent;
        private Dictionary<JointType, PatientJoint> children = new Dictionary<JointType, PatientJoint>();
        private JointType type;

        private string translation;

        private bool xAxis;
        private bool yAxis;
        private bool zAxis;

        private int xAxisMinValue;
        private int xAxisMaxValue;

        private int yAxisMinValue;
        private int yAxisMaxValue;

        private int zAxisMinValue;
        private int zAxisMaxValue;

        public KinectJoint(JointType type,
            string translation,
            bool xAxis,
            bool yAxis,
            bool zAxis,
            int xAxisMinValue,
            int xAxisMaxValue,
            int yAxisMinValue,
            int yAxisMaxValue,
            int zAxisMinValue,
            int zAxisMaxValue)
        {
            this.type = type;
            this.translation = translation;

            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.zAxis = zAxis;

            this.xAxisMinValue = xAxisMinValue;
            this.xAxisMaxValue = xAxisMaxValue;

            this.yAxisMinValue = yAxisMinValue;
            this.yAxisMaxValue = yAxisMaxValue;

            this.zAxisMinValue = zAxisMinValue;
            this.zAxisMaxValue = zAxisMaxValue;
        }

        public JointType JointType
        {
            get
            {
                return type;
            }

            set
            {
                this.type = value;
            }
        }

        public string Translation
        {
            get
            {
                return translation;
            }
        }

        public bool XAxis
        {
            get
            {
                return this.xAxis;
            }

            set
            {
                this.xAxis = value;
            }
        }

        public bool YAxis
        {
            get
            {
                return this.yAxis;
            }

            set
            {
                this.yAxis = value;
            }
        }

        public bool ZAxis
        {
            get
            {
                return this.zAxis;
            }

            set
            {
                this.zAxis = value;
            }
        }

        public int XAxisMinValue
        {
            get
            {
                return this.xAxisMinValue;
            }

            set
            {
                this.xAxisMinValue = value;
            }
        }

        public int XAxisMaxValue
        {
            get
            {
                return this.xAxisMaxValue;
            }

            set
            {
                this.xAxisMaxValue = value;
            }
        }

        public int YAxisMinValue
        {
            get
            {
                return this.yAxisMinValue;
            }

            set
            {
                this.yAxisMinValue = value;
            }
        }

        public int YAxisMaxValue
        {
            get
            {
                return this.yAxisMaxValue;
            }

            set
            {
                this.yAxisMaxValue = value;
            }
        }

        public int ZAxisMinValue
        {
            get
            {
                return this.zAxisMinValue;
            }

            set
            {
                this.zAxisMinValue = value;
            }
        }

        public int ZAxisMaxValue
        {
            get
            {
                return this.zAxisMaxValue;
            }

            set
            {
                this.zAxisMaxValue = value;
            }
        }

        public PatientJoint Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public Dictionary<JointType, PatientJoint> Children
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
            }
        }

        public PatientJoint GetChild(JointType name)
        {
            if (children.ContainsKey(name))
                return children[name];

            return null;
        }

        public static Dictionary<string, Joint> GetJoints(Body body, KinectJoint joint)
        {
            var parent = joint.Parent == null ? joint : joint.Parent;
            var child = joint.Children.Count == 0 ? joint : joint.Children.First().Value;

            return new Dictionary<string, Joint>
            {
                { "base", body.Joints[joint.JointType] },
                { "parent", body.Joints[parent.JointType] },
                { "child", body.Joints[child.JointType] },
            };
        }

        public abstract override object Insert();

        public abstract override IDBObject Select();

        public abstract override bool Update();

        public abstract override void Delete();
    }
}
