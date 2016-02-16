namespace HSA.RehaGame.DB.Models
{
    using Kinect = Windows.Kinect;

    public class PatientJoint : Model
    {
        private string name;
        private bool active = true;

        private long xyMinValue;
        private long xyMaxValue;

        private long yzMinValue;
        private long yzMaxValue;

        private long zxMinValue;
        private long zxMaxValue;

        private Joint joint;

        public PatientJoint(string name)
        {
            this.name = name;
        }

        public PatientJoint(Joint joint)
        {
            this.name = joint.Name;
            this.joint = joint;
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

        [ForeignKey("joint", "joint_id", true)]
        public Joint KinectJoint
        {
            get
            {
                if (joint == null)
                    return null;

                else if (!joint.IsInstance)
                    joint.SetData();

                return this.joint;
            }

            set
            {
                this.joint = value;
            }
        }

        public Kinect.JointType Type
        {
            get
            {
                return this.joint.Type;
            }
        }

        public string Translation
        {
            get
            {
                return this.joint.Translation;
            }
        }

        public void SetActive(bool active)
        {
            // ToDo Von Joint erben
            this.Active = active;
        }
    }
}
