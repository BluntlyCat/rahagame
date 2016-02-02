namespace HSA.RehaGame.InGame
{
    using System.Collections.Generic;
    using Exercises;
    using UnityEngine;
    using User;

    public class GameState : MonoBehaviour
    {
        private static Patient activePatient;
        private static Exercise activeExercise;

        private static IDictionary<string, Exercise> exercises = new Dictionary<string, Exercise>();

        private static bool exerciseIsActive;
        private static bool hasKinectUser;

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

        public static void AddExercise(Exercise exercise)
        {
            exercises.Add(exercise.UnityObjectName, exercise);
        }

        public static void SetActiveExercise(string unityObjectName)
        {
            activeExercise = exercises[unityObjectName];
        }
    }
}