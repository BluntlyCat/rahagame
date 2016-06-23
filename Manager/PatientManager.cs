namespace HSA.RehaGame.Manager
{
    using System;
    using Mono.Data.Sqlite;
    using UnityEngine;
    using Models = DB.Models;
    using System.Collections.Generic;
    using DB.Models;
    using System.Linq;

    public class PatientManager : MonoBehaviour
    {
        private IDictionary<object, Patient> models;
        private Patient activePatient;

        // Use this for initialization
        void Start()
        {
            models = Model.All<Patient>();
        }

        public void SetActivePatient(string name)
        {
            if (models.ContainsKey(name))
                activePatient = models[name];

            throw new Exception(string.Format("There is no patient with name {0}", name));
        }

        public void AddPatient(string name, string age, int sex)
        {
            Patient patient = new Patient(name, int.Parse(age), (Sex)sex);

            var result = patient.Save();

            if(result.ErrorCode == SQLiteErrorCode.Ok)
            {
                models.Add(patient.Name, patient);
                activePatient = patient;
            }
            
        }

        public void SetStressedJoints(Exercise exercise)
        {
            foreach (var joint in exercise.StressedJoints.Values)
                this.ActivePatient.GetJointByName(joint.Name).KinectJoint.Stressed = true;
        }

        public void DeletePatient()
        {

        }

        public void ActivateJoints()
        {
            var jointPanel = GameObject.Find("jointList");

            foreach (Transform joint in jointPanel.transform)
            {
                joint.gameObject.SetActive(true);
            }
        }

        public Models.Patient ActivePatient
        {
            get
            {
                if (activePatient == null)
                    activePatient = Models.Model.All<Patient>().Values.Last();

                return activePatient;
            }

            set
            {
                activePatient = value;
            }
        }
    }
}