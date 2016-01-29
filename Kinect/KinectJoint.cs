namespace HSA.RehaGame.Kinect
{
    using System.Collections.Generic;
    using DB;
    using Kinect = Windows.Kinect;

    public abstract class KinectJoint : DBObject
    {
        private KinectJoint parent;
        private Dictionary<Kinect.JointType, KinectJoint> children = new Dictionary<Kinect.JointType, KinectJoint>();
        private Kinect.JointType type;

        private bool xAxis;
        private bool yAxis;
        private bool zAxis;

        private int xAxisMinValue;
        private int xAxisMaxValue;

        private int yAxisMinValue;
        private int yAxisMaxValue;

        private int zAxisMinValue;
        private int zAxisMaxValue;

        public KinectJoint(Kinect.JointType type,
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

        public Kinect.JointType JointType
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

        public KinectJoint Parent
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

        public Dictionary<Kinect.JointType, KinectJoint> Children
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

        public KinectJoint GetChild(Kinect.JointType name)
        {
            if (children.ContainsKey(name))
                return children[name];

            return null;
        }

        public abstract override object Insert();

        public abstract override IDBObject Select();

        public abstract override bool Update();

        public abstract override void Delete();
    }
}
