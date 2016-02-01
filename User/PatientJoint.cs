namespace HSA.RehaGame.User
{
    using System.Collections.Generic;
    using DB;
    using Kinect;
    using Windows.Kinect;

    public class PatientJoint : KinectJoint
    {
        private int id;
        private int patientjoint_id;

        private string patientName;

        private bool active = true;
        private bool stressed = false;

        private int xAxisPatientMinValue;
        private int xAxisPatientMaxValue;

        private int yAxisPatientMinValue;
        private int yAxisPatientMaxValue;

        private int zAxisPatientMinValue;
        private int zAxisPatientMaxValue;

        public PatientJoint(string patientName, JointType type, string translation, bool xAxis, bool yAxis, bool zAxis, int xAxisMinValue, int xAxisMaxValue, int yAxisMinValue, int yAxisMaxValue, int zAxisMinValue, int zAxisMaxValue) : base(type, translation, xAxis, yAxis, zAxis, xAxisMinValue, xAxisMaxValue, yAxisMinValue, yAxisMaxValue, zAxisMinValue, zAxisMaxValue)
        {
            this.patientName = patientName;

            this.xAxisPatientMinValue = xAxisMinValue;
            this.xAxisPatientMaxValue = xAxisMaxValue;

            this.yAxisPatientMinValue = yAxisMinValue;
            this.yAxisPatientMaxValue = yAxisMaxValue;

            this.zAxisPatientMinValue = zAxisMinValue;
            this.zAxisPatientMaxValue = zAxisMaxValue;
        }

        public int ID
        {
            get
            {
                return id;
            }

            private set
            {
                id = value;
            }
        }

        public int PatientJoint_ID
        {
            get
            {
                return patientjoint_id;
            }

            private set
            {
                patientjoint_id = value;
            }
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

        public bool Stressed
        {
            get
            {
                return stressed;
            }

            set
            {
                stressed = value;
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

            this.Update();
            return children;
        }

        public override object Insert()
        {
            ID = int.Parse(DBManager.Insert("editor_patientjoint",
                    new KeyValuePair<string, object>("active", this.Active),
                    new KeyValuePair<string, object>("x_axis_min_value", this.XAxisMinValue),
                    new KeyValuePair<string, object>("x_axis_max_value", this.XAxisMaxValue),
                    new KeyValuePair<string, object>("y_axis_min_value", this.YAxisMinValue),
                    new KeyValuePair<string, object>("y_axis_max_value", this.YAxisMaxValue),
                    new KeyValuePair<string, object>("z_axis_min_value", this.ZAxisMinValue),
                    new KeyValuePair<string, object>("z_axis_max_value", this.ZAxisMaxValue),
                    new KeyValuePair<string, object>("kinectJoint_id", this.JointType.ToString())
                ));

            PatientJoint_ID = int.Parse(DBManager.Insert("editor_patient_joints",
                new KeyValuePair<string, object>("patient_id", patientName),
                new KeyValuePair<string, object>("patientjoint_id", ID)
            ));

            return ID;
        }

        public override IDBObject Select()
        {
            var joint = DBManager.GetPatientJoint(JointType, patientName);

            ID = joint.GetInt("id");
            Active = joint.GetBool("active");

            XAxisPatientMinValue = joint.GetInt("x_axis_min_value");
            XAxisPatientMinValue = joint.GetInt("x_axis_max_value");

            XAxisPatientMinValue = joint.GetInt("y_axis_min_value");
            XAxisPatientMinValue = joint.GetInt("y_axis_max_value");

            XAxisPatientMinValue = joint.GetInt("z_axis_min_value");
            XAxisPatientMinValue = joint.GetInt("z_axis_max_value");

            return this;
        }

        public override bool Update()
        {
            DBManager.Update(ID.ToString(), "editor_patientjoint",
                new KeyValuePair<string, object>("active", Active),
                new KeyValuePair<string, object>("x_axis_min_value", XAxisMinValue),
                new KeyValuePair<string, object>("x_axis_max_value", XAxisMaxValue),
                new KeyValuePair<string, object>("y_axis_min_value", YAxisMinValue),
                new KeyValuePair<string, object>("y_axis_max_value", YAxisMaxValue),
                new KeyValuePair<string, object>("z_axis_min_value", ZAxisMinValue),
                new KeyValuePair<string, object>("z_axis_max_value", ZAxisMaxValue)
            );

            return true;
        }

        public override void Delete()
        {
            DBManager.Detete("editor_patientjoint", this.ID.ToString());
            DBManager.Detete("editor_patient_joints", this.PatientJoint_ID.ToString());
        }

        public override string ToString()
        {
            return this.JointType + " (" + this.Active + ")";
        }
    }
}