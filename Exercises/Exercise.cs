namespace HSA.RehaGame.Exercises
{
    using System.Collections.Generic;
    using DB;
    using InGame;
    using UnityEngine;
    using User;

    public class Exercise : DBObject
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

        public Exercise(string unityObjectName)
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

        public override object Insert() { return null; }

        public override IDBObject Select()
        {
            var table = DBManager.Query("editor_exercise", string.Format("SELECT * FROM editor_exercise WHERE unityObjectName = '{0}'", this.UnityObjectName));

            this.id = table.GetInt("id");
            this.name = table.GetValueFromLanguage("name");
            this.thumbnail = table.GetResource<Texture>("thumbnail", "jpg", false);
            this.difficulty = (Difficulties)table.GetInt("difficulty");
            this.video = table.GetResource<MovieTexture>("video", "mp4");
            this.description = table.GetValueFromLanguage("description");
            this.auditiveDescription = table.GetResource<AudioClip>("auditiveDescription", "mp3");
            this.rel = table.GetValue("rel");
            this.auditiveInformation = table.GetResource<AudioClip>("auditiveInformation", "mp3");
            this.information = table.GetValueFromLanguage("information");

            var stressedJointsDB = DBManager.GetStressedJoints(this.id);

            foreach(var joint in stressedJointsDB.Rows)
            {
                var name = joint.GetValue("name");
                var stressedJoint = GameState.ActivePatient.GetJoint(name);

                stressedJoint.Stressed = true;
                stressedJoints.Add(name, stressedJoint);
            }

            return this;
        }

        public override bool Update() { return true; }
    }
}
