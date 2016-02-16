namespace HSA.RehaGame.Manager
{
    using System;
    using System.Collections.Generic;
    using Mono.Data.Sqlite;
    using UnityEngine;
    using Models = DB.Models;

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

            switch(errorCode)
            {
                case SQLiteErrorCode.Ok:
                    errorCode = CreatePatientJoints(patient);

                    switch(errorCode)
                    {
                        case SQLiteErrorCode.Ok:
                            // ToDo Ebenso ExercisesToDo erstellen, aus QRCode
                            // Patient aus QRCode erstellen
                            errorCode = patient.AddManyToManyRelations();

                            switch(errorCode)
                            {
                                case SQLiteErrorCode.Ok:
                                    models.Add(patient.Name, patient);
                                    activePatient = patient;
                                    break;

                                default:
                                    patient.Delete();
                                    // ToDo Bei Fehler aufräumen und alles entfernen
                                    break;
                            }
                            
                            break;

                        default:
                            patient.Delete();
                            break;
                    }

                    break;

                case SQLiteErrorCode.Constraint:
                    // Todo Fehler anzeigen, dass Benutzer existiert
                    break;
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
                var patientJoint = new Models.PatientJoint(joint);
                errorCode = patientJoint.Save();

                switch(errorCode)
                {
                    case SQLiteErrorCode.Ok:
                        patientJoints.Add(joint.Name, patientJoint);
                        break;

                    default:
                        foreach (var addedPatientJoint in patientJoints.Values)
                        {
                            addedPatientJoint.Delete();
                        }

                        return SQLiteErrorCode.Error;
                }
            }

            if (errorCode == SQLiteErrorCode.Ok)
                patient.Joints = patientJoints;

            return errorCode;
        }
    }
}