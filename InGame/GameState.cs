namespace HSA.RehaGame.InGame
{
    using User;
    using UnityEngine;

    public class GameState : MonoBehaviour
    {
        private static string patientName;
        private static Patient activePatient;

        public static string PatientName
        {
            get
            {
                return patientName;
            }

            set
            {
                patientName = value;
            }
        }

        public static Patient ActivePatient
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
    }
}