namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;

    public class Exercise : UnityModel
    {
        private string name;
        private string auditiveName;
        private string thumbnail;
        private long difficulty;
        private string video;
        private string description;
        private string auditiveDescription;
        private string information;
        private string auditiveInformation;
        private string reml;

        private Dictionary<string, Exercise> similarExercises;
        private Dictionary<string, Equipment> requiredEquipments;
        private Dictionary<string, Joint> stressedJoints;
        private Dictionary<string, Music> playlist;

        public Exercise(string unityObjectName) : base(unityObjectName)
        {

        }

        [TranslationColumn]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.name = value;
            }
        }

        [TranslationColumn]
        [ResourceColumn]
        public string AuditiveName
        {
            get
            {
                return auditiveName;
            }

            set
            {
                this.auditiveName = value;
            }
        }

        [ResourceColumn]
        public string Thumbnail
        {
            get
            {
                return thumbnail;
            }

            set
            {
                this.thumbnail = value;
            }
        }

        [TableColumn]
        public long Difficulty
        {
            get
            {
                return difficulty;
            }

            set
            {
                this.difficulty = value;
            }
        }

        [TranslationColumn]
        [ResourceColumn]
        public string Video
        {
            get
            {
                return video;
            }

            set
            {
                this.video = value;
            }
        }

        [TranslationColumn]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.description = value;
            }
        }

        [TranslationColumn]
        [ResourceColumn]
        public string AuditiveDescription
        {
            get
            {
                return auditiveDescription;
            }

            set
            {
                this.auditiveDescription = value;
            }
        }

        [TranslationColumn]
        public string Information
        {
            get
            {
                return information;
            }

            set
            {
                this.information = value;
            }
        }

        [TranslationColumn]
        [ResourceColumn]
        public string AuditiveInformation
        {
            get
            {
                return auditiveInformation;
            }

            set
            {
                this.auditiveInformation = value;
            }
        }

        [TableColumn]
        public string Reml
        {
            get
            {
                return reml;
            }

            set
            {
                this.reml = value;
            }
        }

        [ManyToManyRelation(
            "unityObjectName",
            "exercise",
            "from_exercise_id",
            "exercise_similarExercises",
            "to_exercise_id",
            "exercise",
            "unityObjectName"
        )]
        public Dictionary<string, Exercise> SimilarExercises
        {
            get
            {
                return this.similarExercises;
            }

            set
            {
                this.similarExercises = value;
            }
        }

        [ManyToManyRelation(
            "unityObjectName",
            "exercise",
            "exercise_id",
            "exercise_requiredEquipments",
            "equipment_id",
            "equipment",
            "unityObjectName"
        )]
        public Dictionary<string, Equipment> RequiredEquipments
        {
            get
            {
                return this.requiredEquipments;
            }

            set
            {
                this.requiredEquipments = value;
            }
        }

        [ManyToManyRelation(
            "unityObjectName",
            "exercise",
            "exercise_id",
            "exercise_stressedJoints",
            "joint_id",
            "joint",
            "name"
        )]
        public Dictionary<string, Joint> StressedJoints
        {
            get
            {
                return this.stressedJoints;
            }

            set
            {
                this.stressedJoints = value;
            }
        }

        [ManyToManyRelation(
            "unityObjectName",
            "exercise",
            "exercise_id",
            "exercise_playlist",
            "music_id",
            "music",
            "unityObjectName"
        )]
        public Dictionary<string, Music> Playlist
        {
            get
            {
                return playlist;
            }

            set
            {
                this.playlist = value;
            }
        }

        // ToDo Equipment
    }
}
