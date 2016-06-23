namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
    using Mono.Data.Sqlite;

    public class Patient : Model
    {
        private string name;
        private long age;
        private long sex;

        private Dictionary<long, PatientJoint> joints;
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
            "id"
        )]
        public Dictionary<long, PatientJoint> Joints
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

        public override TransactionResult Save()
        {
            var result = base.Save();

            switch (result.ErrorCode)
            {
                case SQLiteErrorCode.Ok:
                    result = this.CreatePatientJoints();

                    switch (result.ErrorCode)
                    {
                        case SQLiteErrorCode.Ok:
                            // ToDo Ebenso ExercisesToDo erstellen, aus QRCode
                            // Patient aus QRCode erstellen
                            result = this.AddManyToManyRelations();

                            switch (result.ErrorCode)
                            {
                                case SQLiteErrorCode.Ok:
                                    break;

                                default:
                                    foreach (var joint in this.joints.Values)
                                        joint.Delete();

                                    this.Delete();
                                    break;
                            }

                            break;

                        default:
                            this.Delete();
                            break;
                    }

                    break;

                case SQLiteErrorCode.Constraint:
                    // Todo Fehler anzeigen, dass Benutzer existiert
                    break;
            }

            return result;
        }

        private TransactionResult CreatePatientJoints()
        {
            TransactionResult result = new TransactionResult(SQLiteErrorCode.Ok, null);
            Dictionary<long, PatientJoint> patientJoints = new Dictionary<long, PatientJoint>();

            var joints = All<KinectJoint>();

            foreach (var joint in joints.Values)
            {
                var patientJoint = new PatientJoint(joint);
                result = patientJoint.Save();

                switch (result.ErrorCode)
                {
                    case SQLiteErrorCode.Ok:
                        patientJoints.Add((long)result.PrimaryKeyValue, patientJoint);
                        break;

                    default:
                        foreach (var addedPatientJoint in patientJoints.Values)
                            addedPatientJoint.Delete();

                        return new TransactionResult(SQLiteErrorCode.Error, null);
                }
            }

            if (result.ErrorCode == SQLiteErrorCode.Ok)
                this.Joints = patientJoints;

            return result;
        }

        public PatientJoint GetJointByName(string name)
        {
            foreach(var joint in this.Joints.Values)
            {
                if (joint.KinectJoint.Name == name)
                    return joint;
            }

            return null;
        }
    }
}
