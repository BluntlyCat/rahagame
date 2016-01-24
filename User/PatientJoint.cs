namespace HSA.RehaGame.User
{
    using UnityEngine;
    using System.Collections;
    using Kinect;
    using Settings;

    public class PatientJoint : MonoBehaviour
    {
        private RGJoint joint;
        private enum states { active, inactive, calibrating }

        private Color[] colors = {
            new Color(0, 196, 255, 255),
            new Color(255, 0, 0, 255),
            new Color(255, 255, 0, 255)
        };

        void Start()
        {

        }

        // Use this for initialization
        void OnEnable()
        {
            if(RGSettings.ActivePatient != null)
            {

            }
        }

        public void Activate()
        {
            Debug.Log("activate");

            if (joint != null)
                joint.Activate(!joint.Active);
        }
    }
}