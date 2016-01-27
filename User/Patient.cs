namespace HSA.RehaGame.User
{
    using System.Collections.Generic;
    using System.Linq;
    using Kinect = Windows.Kinect;
    using Kinect;
    using DB;
    using Scene;

    public class Patient
    {
        private string name;
        private string age;
        private string sex;

        private IDictionary<Kinect.JointType, PatientJoint> joints;
        private bool insert = true;

        public static Patient Instance(string name)
        {
            if (DBManager.Exists("editor_patient", name))
            {
                var patientData = DBManager.Query("SELECT * FROM editor_patient WHERE name ='" + name + "';").First();
                var patientJointIDs = DBManager.Query("SELECT patientjoint_id FROM editor_patient_joints WHERE patient_id ='" + name + "';");

                return new Patient(patientData["name"].ToString(), patientData["age"].ToString(), patientData["sex"].ToString(), patientJointIDs);
            }

            return null;
        }

        public Patient(string name, string age, string sex)
        {
            this.name = name;
            this.age = age;
            this.sex = sex;

            this.joints = RGJoints.Copy();
        }

        public Patient(string name, string age, string sex, IList<Dictionary<string, object>> patientJointIDs)
        {
            this.name = name;
            this.age = age;
            this.sex = sex;

            this.joints = RGJoints.Copy(patientJointIDs);
        }

        public Patient(string name, string age, string sex, IDictionary<Kinect.JointType, PatientJoint> joints)
        {
            this.name = name;
            this.age = age;
            this.sex = sex;

            this.joints = joints;
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public string Sex
        {
            get
            {
                return this.sex;
            }

            set
            {
                this.sex = value;
            }
        }

        public string Age
        {
            get
            {
                return this.age;
            }

            set
            {
                this.age = value;
            }
        }

        public bool ActivateJoint(string name)
        {
            var jt = (Kinect.JointType)System.Enum.Parse(typeof(Kinect.JointType), name);
            this.joints[jt].Active = !this.joints[jt].Active;

            return this.joints[jt].Active;
        }

        public PatientJoint GetJoint(string name)
        {
            var jt = (Kinect.JointType)System.Enum.Parse(typeof(Kinect.JointType), name);
            return this.joints[jt];
        }

        public bool Save(bool update)
        {
            if (update)
            {
                DBManager.Update(this.Name, "editor_patient",
                    new KeyValuePair<string, object>("age", this.age),
                    new KeyValuePair<string, object>("sex", this.sex)
                );
            }
            else
            {
                if (DBManager.Exists("editor_patient", this.Name))
                    return false;

                var id = DBManager.Insert("editor_patient",
                    new KeyValuePair<string, object>("name", this.Name),
                    new KeyValuePair<string, object>("age", this.Age),
                    new KeyValuePair<string, object>("sex", this.Sex)
                );

                foreach (var joint in this.joints.Values)
                {
                    var jid = DBManager.Insert("editor_patientjoint",
                        new KeyValuePair<string, object>("active", joint.Active.ToString().ToLower()),
                        new KeyValuePair<string, object>("x_axis_min_value", joint.XAxisMinValue),
                        new KeyValuePair<string, object>("x_axis_max_value", joint.XAxisMaxValue),
                        new KeyValuePair<string, object>("y_axis_min_value", joint.YAxisMinValue),
                        new KeyValuePair<string, object>("y_axis_max_value", joint.YAxisMaxValue),
                        new KeyValuePair<string, object>("z_axis_min_value", joint.ZAxisMinValue),
                        new KeyValuePair<string, object>("z_axis_max_value", joint.ZAxisMaxValue),
                        new KeyValuePair<string, object>("kinectJoint_id", joint.Type.ToString())
                    );

                    DBManager.Insert("editor_patient_joints",
                        new KeyValuePair<string, object>("patient_id", id),
                        new KeyValuePair<string, object>("patientjoint_id", jid)
                    );
                }
            }

            return true;
        }

        public void Delete()
        {
            if (DBManager.Exists("editor_patient", this.Name))
            {
                var patientJointMap = DBManager.Query("SELECT * FROM editor_patient_joints WHERE patient_id ='" + this.Name + "';");

                foreach (var patientJointMapping in patientJointMap)
                {
                    var patientJoint = DBManager.Query("SELECT * FROM editor_patientjoint WHERE id = '" + patientJointMapping["patientjoint_id"] + "';").First();
                    DBManager.Detete("editor_patientjoint", patientJointMapping["patientjoint_id"].ToString());
                    DBManager.Detete("editor_patient_joints", patientJointMapping["id"].ToString());
                }

                DBManager.Detete("editor_patient", this.Name);
                LoadScene.GoOneSceneBack();
            }
        }
    }
}
