namespace HSA.RehaGame.Manager
{
    using System;
    using System.Collections.Generic;
    using Models = DB.Models;
    using Mono.Data.Sqlite;
    using UnityEngine;

    public class PatientManager : BaseModelManager<Models.Patient>
    {
        private SettingsManager settingsManager;
        private SceneManager sceneManager;
        private Models.Patient activePatient;

        // Use this for initialization
        void Start()
        {
            settingsManager = this.GetComponent<SettingsManager>();
            sceneManager = this.GetComponent<SceneManager>();
        }

        public void SetActivePatient(string name)
        {
            if (models.ContainsKey(name))
                activePatient = models[name];

            throw new Exception(string.Format("There is no patient with name {0}", name));
        }

        public void AddPatient(string name, string age, int sex)
        {
            Models.Patient patient = new Models.Patient(name, int.Parse(age), (Models.Sex)sex);

            var errorCode = patient.Save();

            if (errorCode == SQLiteErrorCode.Ok)
            {
                // ToDo Ebenso ExercisesToDo erstellen, aus QRCode
                // Patient aus QRCode erstellen
                if (CreatePatientJoints(patient) == SQLiteErrorCode.Ok)
                {
                    models.Add(patient.Name, patient);
                    activePatient = patient;
                }
            }
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
                return activePatient;
            }

            set
            {
                activePatient = value;
            }
        }

        private SQLiteErrorCode CreatePatientJoints(Models.Patient patient)
        {
            SQLiteErrorCode errorCode = SQLiteErrorCode.Ok;
            Dictionary<string, Models.PatientJoint> patientJoints = new Dictionary<string, Models.PatientJoint>();

            var joints = Models.Model.All<Models.Joint>();

            foreach(var joint in joints.Values)
            {
                var patientJoint = new Models.PatientJoint(joint.Name);

                patientJoint.SetData();
                errorCode = patientJoint.Save();

                if (errorCode == SQLiteErrorCode.Ok)
                    patientJoints.Add(joint.Name, patientJoint);
            }

            return errorCode;
        }
    }
}