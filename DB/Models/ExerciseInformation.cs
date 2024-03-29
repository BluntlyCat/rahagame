﻿namespace HSA.RehaGame.DB.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class ExerciseInformation : UnityModel
    {
        private string exerciseType;
        private string order;
        private AudioClip auditiveOrder;
        
        public ExerciseInformation(string unityObjectName) : base (unityObjectName) {}

        [TableColumn]
        public string Type
        {
            get
            {
                return exerciseType;
            }

            set
            {
                this.exerciseType = value;
            }
        }

        [TranslationColumn]
        public string Order
        {
            get
            {
                return order;
            }

            set
            {
                this.order = value;
            }
        }

        [TranslationColumn]
        [Resource]
        public AudioClip AuditiveOrder
        {
            get
            {
                return auditiveOrder;
            }

            set
            {
                this.auditiveOrder = value;
            }
        }
    }
}
