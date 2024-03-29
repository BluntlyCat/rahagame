﻿namespace HSA.RehaGame.DB.Models
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Exercise : UnityModel
    {
        private string name;
        private AudioClip auditiveName;
        private Texture2D thumbnail;
        private long difficulty;
        private MovieTexture video;
        private string description;
        private AudioClip auditiveDescription;
        private string information;
        private AudioClip auditiveInformation;
        private string reml;

        private Dictionary<string, Exercise> similarExercises;
        private Dictionary<string, Equipment> requiredEquipments;
        private Dictionary<string, KinectJoint> stressedJoints;
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
        [Resource]
        public AudioClip AuditiveName
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

        [TableColumn]
        [Resource]
        public Texture2D Thumbnail
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
        [Resource]
        public MovieTexture Video
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
        [Resource]
        public AudioClip AuditiveDescription
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
        [Resource]
        public AudioClip AuditiveInformation
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
            "kinectjoint_id",
            "kinectjoint",
            "name"
        )]
        public Dictionary<string, KinectJoint> StressedJoints
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

        public void SetStressedJoints()
        {
            foreach (var joint in this.stressedJoints.Values)
                joint.Stressed = true;
        }
    }
}
