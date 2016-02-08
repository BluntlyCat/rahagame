namespace HSA.RehaGame.User
{
    using System.Collections.Generic;
    using Windows.Kinect;
    using DB;
    using Scene;
    using Kinect;
    using DB.Entities;
    using DB.Queries;
    using System;

    public class Patient : DBObject<string>
    {
        private SceneManager sceneManager;
        private string name;
        private long age;
        private Sex sex;

        private Dictionary<JointType, PatientJoint> joints;

        public Patient(string name, SceneManager sceneManager, Database dbManager) : base (dbManager)
        {
            this.name = name;
            this.sceneManager = sceneManager;
            this.joints = new KinectJointManager(dbManager, name).Joints;
        }

        public Patient(string name, int age, Sex sex, SceneManager sceneManager, Database dbManager) : base (dbManager)
        {
            this.name = name;
            this.age = age;
            this.sex = sex;

            this.sceneManager = sceneManager;
            this.joints = new KinectJointManager(dbManager, name).Joints;
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

        public Sex Sex
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

        public long Age
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
            var jt = (JointType)System.Enum.Parse(typeof(JointType), name);
            this.joints[jt].Active = !this.joints[jt].Active;

            return this.joints[jt].Active;
        }

        public void ResetJoints()
        {
            foreach(var joint in joints.Values)
            {
                joint.Stressed = false;
            }
        }

        public PatientJoint GetJoint(string name)
        {
            var jt = (JointType)System.Enum.Parse(typeof(JointType), name);
            return this.joints[jt];
        }

        public PatientJoint GetJoint(JointType jt)
        {
            return this.joints[jt];
        }

        public override PrimaryKey<string> Insert()
        {
            if (database.SelectWhere("editor_patient", "name", this.Name) != null)
                return null;

            var id = database.InsertInto(new InsertQuery<string>(
                "editor_patient",
                new PrimaryKey<string>("name", this.name),
                new QueryColumns("name", "age", "sex"),
                new QueryValues(this.name, this.age, this.sex)
            ));

            foreach (var joint in this.joints.Values)
            {
                joint.Insert();
            }

            return id;
        }

        public override IDBObject Select()
        {
            if (dbManager.Exists("editor_patient", name))
            {
                var patientData = dbManager.Query("editor_patient", "SELECT age, sex FROM editor_patient WHERE name ='" + Name + "';").GetRow();

                this.Age = patientData.GetColumn<long>("age");
                this.Sex = (Sex)patientData.GetColumn<int>("sex");

                foreach(var joint in this.joints.Values)
                {
                    joint.Select();
                }

                return this;
            }

            return null;
        }

        public override bool Update()
        {
            dbManager.UpdateTable(this.Name, "editor_patient",
                    new KeyValuePair<string, object>("age", this.age),
                    new KeyValuePair<string, object>("sex", this.sex)
                );

            return true;
        }

        public override void Delete()
        {
            if (dbManager.Exists("editor_patient", this.Name))
            {
                var patientJointMap = dbManager.Query("editor_patient_joints", "SELECT * FROM editor_patient_joints WHERE patient_id ='" + this.Name + "';");

                foreach (var patientJointMapping in patientJointMap.Rows)
                {
                    dbManager.Detete("editor_patientjoint", patientJointMapping.GetValue("patientjoint_id"));
                    dbManager.Detete("editor_patient_joints", patientJointMapping.GetValue("id"));
                }

                dbManager.Detete("editor_patient", this.Name);
                sceneManager.LoadUsersSlection();
            }
        }
    }
}
