namespace HSA.RehaGame.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Kinect = Windows.Kinect;
    using System.Text;
    using Kinect;
    using DB;
    using Scene;

    public class Patient
    {
        private string name;
        private string age;
        private string sex;

        private IDictionary<Kinect.JointType, RGJoint> joints;
        private bool insert = true;

        public static Patient Instance(string name)
        {
            if(DBManager.Exists("editor_player", name))
            {
                var patientData = DBManager.Query("SELECT * FROM editor_player WHERE name ='" + name + "';").First();

                return new Patient(patientData["name"].ToString(), patientData["age"].ToString(), patientData["sex"].ToString());
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

        public bool Save(bool update)
        {
            if(update)
            {
                DBManager.Update(this.Name, "editor_player",
                    new KeyValuePair<string, object>("age", this.age),
                    new KeyValuePair<string, object>("sex", this.sex)
                );
            }
            else
            {
                if (DBManager.Exists("editor_player", this.Name))
                    return false;

                DBManager.Insert("editor_player", this.Name, this.Age, this.Sex);
            }

            return true;
        }

        public void Delete()
        {
            if (DBManager.Exists("editor_player", this.Name))
            {
                DBManager.Detete("editor_player", this.Name);
                LoadScene.GoOneSceneBack();
            }
        }
    }
}
