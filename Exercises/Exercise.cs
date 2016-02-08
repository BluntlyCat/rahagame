namespace HSA.RehaGame.Exercises
{
    using System.Collections.Generic;
    using DB;
    using DB.Entities;
    using InGame;
    using UnityEngine;
    using User;

    public class Exercise : DBObject<int>
    {
        private int id;
        private string unityObjectName;
        private string name;
        private Texture thumbnail;
        private Difficulties difficulty;
        private MovieTexture video;
        private string description;
        private AudioClip auditiveDescription;
        private string rel;
        private AudioClip auditiveInformation;
        private string information;
        
        private IDictionary<string, PatientJoint> stressedJoints = new Dictionary<string, PatientJoint>();

        public Exercise(string unityObjectName, IDatabase database) : base(database)
        {
            this.unityObjectName = unityObjectName;
        }

        public int ID
        {
            get
            {
                return id;
            }
        }

        public string UnityObjectName
        {
            get
            {
                return unityObjectName;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Texture Thumbnail
        {
            get
            {
                return thumbnail;
            }
        }

        public Difficulties Difficulty
        {
            get
            {
                return difficulty;
            }
        }

        public MovieTexture Video
        {
            get
            {
                return video;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public AudioClip AuditiveDescription
        {
            get
            {
                return auditiveDescription;
            }
        }

        public string REL
        {
            get
            {
                return rel;
            }
        }

        public AudioClip AuditiveInformation
        {
            get
            {
                return auditiveInformation;
            }
        }

        public string Information
        {
            get
            {
                return information;
            }
        }

        public IDictionary<string, PatientJoint> StressedJoints
        {
            get
            {
                return stressedJoints;
            }
        }

        public override void Delete() {}

        public override PrimaryKey<int> Insert() { return null; }

        public override IDBObject<int> Select()
        {
            var exercise = database.FindRow("editor_exercise", this.unityObjectName);

            // ToDo: Informationen müssen noch aus Sprachfeldern geladen werden

            this.id = exercise.Column("id").GetValue<int>();
            this.name = exercise.Column("name").GetValue<string>();
            this.thumbnail = Resources.Load<Texture>(exercise.Column("thumbnail").GetValue<string>());
            this.difficulty = (Difficulties)exercise.Column("difficulty").GetValue<int>();
            this.video = Resources.Load<MovieTexture>(exercise.Column("video").GetValue<string>());
            this.description = exercise.Column("description").GetValue<string>();
            this.auditiveDescription = Resources.Load<AudioClip>(exercise.Column("auditiveDescription").GetValue<string>());
            this.rel = exercise.Column("rel").GetValue<string>(); //ToDo: rel heißt in neuer DB reml
            this.auditiveInformation = Resources.Load<AudioClip>(exercise.Column("auditiveInformation", "mp3").GetValue<string>());
            this.information = exercise.Column("information").GetValue<string>();

            var stressedJointsRows = database.Join(this.id, "editor_exercise_stressedjoints", "exercise_id", "joint_id", "editor_joint");

            foreach(var joint in stressedJointsRows)
            {
                var name = joint.Column("name").GetValue<string>();
                var stressedJoint = GameState.ActivePatient.GetJoint(name);

                stressedJoint.Stressed = true;
                stressedJoints.Add(name, stressedJoint);
            }

            return this;
        }

        public override bool Update() { return true; }
    }
}
