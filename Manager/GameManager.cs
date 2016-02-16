namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using UnityEngine;

    [RequireComponent(typeof(SettingsManager))]
    [RequireComponent(typeof(SceneManager))]
    [RequireComponent(typeof(PatientManager))]
    public class GameManager : MonoBehaviour
    {
        private static Exercise activeExercise;

        private static bool exerciseIsActive;
        private static bool hasKinectUser;

        private static double executionTime;

        public static Exercise ActiveExercise
        {
            get
            {
                return activeExercise;
            }

            set
            {
                activeExercise = value;
            }
        }

        public static bool ExerciseIsActive
        {
            get
            {
                return exerciseIsActive;
            }

            set
            {
                exerciseIsActive = value;
            }
        }

        public static bool HasKinectUser
        {
            get
            {
                return hasKinectUser;
            }

            set
            {
                hasKinectUser = value;
            }
        }

        public static double ExecutionTime
        {
            get
            {
                return executionTime;
            }

            set
            {
                executionTime = value;
            }
        }
    }
}
