namespace HSA.RehaGame.InGame
{
    using User;
    using UnityEngine;
    using Exercises;
    using System.Collections.Generic;

    public class GameState : MonoBehaviour
    {
        private static Patient activePatient;
        private static Exercise activeExercise;
        private static IDictionary<string, Exercise> exercises = new Dictionary<string, Exercise>();

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