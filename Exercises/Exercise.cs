namespace HSA.RehaGame.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DB;
    using UnityEngine;

    public class Exercise : DBObject
    {
        private string unityObjectName;

        private string name;
        private Texture thumbnail;
        private Difficulties difficulty;
        private MovieTexture video;
        private string description;
        private AudioClip auditiveDescription;
        private ExecutionLanguage rel;
        private AudioClip auditiveInformation;
        private string information;

        public Exercise(string unityObjectName)
        {
            this.unityObjectName = unityObjectName;
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

        public ExecutionLanguage REL
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

        public override void Delete() {}

        public override object Insert() { return null; }

        public override IDBObject Select()
        {
            var table = DBManager.Query("editor_exercise", string.Format("SELECT * FROM editor_exercise WHERE unityObjectName = '{0}'", this.UnityObjectName));

            this.name = table.GetValueFromLanguage("name");
            this.thumbnail = Resources.Load(table.GetResource("thumbnail", "jpg", false)) as Texture;
            this.difficulty = (Difficulties)table.GetInt("difficulty");
            this.video = Resources.Load(table.GetResource("video", "mp4")) as MovieTexture;
            this.description = table.GetValueFromLanguage("description");
            this.auditiveDescription = Resources.Load(table.GetResource("auditiveDescription", "mp3")) as AudioClip;
            this.rel = new ExecutionLanguage(table.GetValue("rel"));
            this.auditiveInformation = Resources.Load(table.GetResource("auditiveInformation", "mp3")) as AudioClip;
            this.information = table.GetValueFromLanguage("information");

            return this;
        }

        public override bool Update() { return true; }
    }
}
