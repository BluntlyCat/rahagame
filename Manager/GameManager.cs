namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    [RequireComponent(typeof(SettingsManager))]
    [RequireComponent(typeof(SceneManager))]
    [RequireComponent(typeof(PatientManager))]
    public class GameManager : MonoBehaviour
    {
        private static Exercise activeExercise;

        private static bool exerciseIsActive;
        private static bool hasKinectUser;

        private static Dictionary<string, TimeSpan> executionTimes;

        public static Exercise ActiveExercise
        {
            get
            {
                if (activeExercise == null)
                    activeExercise = Model.GetModel<Exercise>("test");

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

        public static Dictionary<string, TimeSpan> ExecutionTimes
        {
            get
            {
                return executionTimes;
            }

            set
            {
                executionTimes = value;
            }
        }
    }
}
