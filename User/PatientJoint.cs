namespace HSA.RehaGame.User
{
    using System.Collections.Generic;
    using System.Linq;
    using Kinect;

    public class PatientJoint : RGJoint
    {
        private bool active = true;

        private int xAxisPatientMinValue;
        private int xAxisPatientMaxValue;

        private int yAxisPatientMinValue;
        private int yAxisPatientMaxValue;

        private int zAxisPatientMinValue;
        private int zAxisPatientMaxValue;

        public PatientJoint(Windows.Kinect.JointType type, string xAxis, string yAxis, string zAxis, string xAxisMinValue, string xAxisMaxValue, string yAxisMinValue, string yAxisMaxValue, string zAxisMinValue, string zAxisMaxValue) : base(type, xAxis, yAxis, zAxis, xAxisMinValue, xAxisMaxValue, yAxisMinValue, yAxisMaxValue, zAxisMinValue, zAxisMaxValue)
        {
            this.xAxisPatientMinValue = int.Parse(xAxisMinValue);
            this.xAxisPatientMaxValue = int.Parse(xAxisMaxValue);

            this.yAxisPatientMinValue = int.Parse(yAxisMinValue);
            this.yAxisPatientMaxValue = int.Parse(yAxisMaxValue);

            this.zAxisPatientMinValue = int.Parse(zAxisMinValue);
            this.zAxisPatientMaxValue = int.Parse(zAxisMaxValue);
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public int XAxisPatientMinValue
        {
            get
            {
                return this.xAxisPatientMinValue;
            }

            set
            {
                this.xAxisPatientMinValue = value;
            }
        }

        public int XAxisPatientMaxValue
        {
            get
            {
                return this.xAxisPatientMaxValue;
            }

            set
            {
                this.xAxisPatientMaxValue = value;
            }
        }

        public int YAxisPatientMinValue
        {
            get
            {
                return this.yAxisPatientMinValue;
            }

            set
            {
                this.yAxisPatientMinValue = value;
            }
        }

        public int YAxisPatientMaxValue
        {
            get
            {
                return this.yAxisPatientMaxValue;
            }

            set
            {
                this.yAxisPatientMaxValue = value;
            }
        }

        public int ZAxisPatientMinValue
        {
            get
            {
                return this.zAxisPatientMinValue;
            }

            set
            {
                this.zAxisPatientMinValue = value;
            }
        }

        public int ZAxisPatientMaxValue
        {
            get
            {
                return this.zAxisPatientMaxValue;
            }

            set
            {
                this.zAxisPatientMaxValue = value;
            }
        }

        public IList<PatientJoint> Activate(bool active)
        {
            List<PatientJoint> children = new List<PatientJoint>();

            this.Active = active;

            foreach (var child in this.Children)
            {
                var childJoint = child.Value as PatientJoint;

                children.Add(childJoint);
                children.AddRange(childJoint.Activate(active));
            }

            return children;
        }

        public override string ToString()
        {
            return this.Type + " (" + this.Active + ")";
        }
    }
}