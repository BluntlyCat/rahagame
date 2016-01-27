namespace HSA.RehaGame.Kinect
{
    using System.Collections.Generic;
    using Kinect = Windows.Kinect;

    public class RGJoint
    {
        private RGJoint parent;
        private Dictionary<Kinect.JointType, RGJoint> children = new Dictionary<Kinect.JointType, RGJoint>();
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

        public RGJoint(Kinect.JointType type,
            string xAxis,
            string yAxis,
            string zAxis,
            string xAxisMinValue,
            string xAxisMaxValue,
            string yAxisMinValue,
            string yAxisMaxValue,
            string zAxisMinValue,
            string zAxisMaxValue)
        {
            this.type = type;

            this.xAxis = xAxis == "true";
            this.yAxis = yAxis == "true";
            this.zAxis = zAxis == "true";

            this.xAxisMinValue = int.Parse(xAxisMinValue);
            this.xAxisMaxValue = int.Parse(xAxisMaxValue);

            this.yAxisMinValue = int.Parse(yAxisMinValue);
            this.yAxisMaxValue = int.Parse(yAxisMaxValue);

            this.zAxisMinValue = int.Parse(zAxisMinValue);
            this.zAxisMaxValue = int.Parse(zAxisMaxValue);
        }

        public Kinect.JointType Type
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

        public RGJoint Parent
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

        public Dictionary<Kinect.JointType, RGJoint> Children
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

        public RGJoint GetChild(Kinect.JointType name)
        {
            if (children.ContainsKey(name))
                return children[name];

            return null;
        }
    }
}
