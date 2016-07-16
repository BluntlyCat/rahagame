namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using System.Collections.Generic;
    using UI.Statistic;
    using UnityEngine;

    [RequireComponent(typeof(SettingsManager))]
    [RequireComponent(typeof(SceneManager))]
    [RequireComponent(typeof(PatientManager))]
    public class GameManager : MonoBehaviour
    {
        private static Patient activePatient;
        private static Exercise activeExercise;

        private static bool exerciseIsActive;
        private static bool hasKinectUser;

        private static List<StatisticData> statistic;
        private static StatisticViewData statisticViewData;

        void Start()
        {
            statisticViewData = StatisticViewData.Instance;
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

        public static StatisticViewData StatisticViewData
        {
            get
            {

                return statisticViewData;
            }
        }
    }
}
