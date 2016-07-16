namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using Mono.Data.Sqlite;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class PatientManager : MonoBehaviour
    {
        private IDictionary<object, Patient> models;

        // Use this for initialization
        void Start()
        {
            models = Model.All<Patient>();
        }

        public void SetActivePatient(string name)
        {
            if (models.ContainsKey(name))
                GameManager.ActivePatient = models[name];

            throw new Exception(string.Format("There is no patient with name {0}", name));
        }

        public void AddPatient(string name, string age, int sex)
        {
            Patient patient = new Patient(name, int.Parse(age), (Sex)sex);

            var result = patient.Save();

            if(result.ErrorCode == SQLiteErrorCode.Ok)
            {
                models.Add(patient.Name, patient);
                GameManager.ActivePatient = patient;
            }
            
        }

        public void SetStressedJoints(Exercise exercise)
        {
            foreach (var joint in exercise.StressedJoints.Values)
                GameManager.ActivePatient.GetJointByName(joint.Name).KinectJoint.Stressed = true;
        }

        public void DeletePatient()
        {
            if (GameManager.ActivePatient != null)
                GameManager.ActivePatient.Delete();
        }

        public void ActivateJoints()
        {
            var jointPanel = GameObject.Find("jointList");

            foreach (Transform joint in jointPanel.transform)
            {
                joint.gameObject.SetActive(true);
            }
        }
    }
}