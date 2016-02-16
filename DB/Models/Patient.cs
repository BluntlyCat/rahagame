namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;

    public class Patient : Model
    {
        private string name;
        private long age;
        private long sex;

        private Dictionary<string, PatientJoint> joints;
        private Dictionary<string, Exercise> exercisesToDo;

        public Patient(string name)
        {
            this.name = name;
        }

        public Patient(string name, long age, Sex sex)
        {
            this.name = name;
            this.age = age;
            this.sex = (long)sex;
        }

        [PrimaryKey]
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

        [TableColumn]
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

        [TableColumn("sex")]
        private long LongSex
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

        public Sex Sex
        {
            get
            {
                return (Sex)this.sex;
            }

            set
            {
                this.sex = (long)value;
            }
        }
        
        [ManyToManyRelation(
            "name",
            "patient",
            "patient_id",
            "patient_joints",
            "patientjoint_id",
            "patientjoint",
            "name"
        )]
        public Dictionary<string, PatientJoint> Joints
        {
            get
            {
                return this.joints;
            }

            set
            {
                this.joints = value;
            }
        }

        [ManyToManyRelation(
            "name",
            "patient",
            "patient_id",
            "patient_exercises_to_do",
            "exercise_id",
            "exercise",
            "unityObjectName"
        )]
        public Dictionary<string, Exercise> ExercisesToDo
        {
            get
            {
                return exercisesToDo;
            }

            set
            {
                this.exercisesToDo = value;
            }
        }
    }
}
