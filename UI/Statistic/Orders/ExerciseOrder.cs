namespace HSA.RehaGame.UI.Statistic.Orders
{
    using DB.Models;
    using Manager;
    using UnityEngine;
    public class ExerciseOrder : StatisticOrder
    {
        public ExerciseOrder(PatientManager patientManager, GameObject buttonPrefab, GameObject buttonStorage) : base(patientManager, buttonPrefab, buttonStorage)
        {
            var exercises = StatisticData.Exercises(GameManager.ActivePatient);

            foreach (var exerciseId in exercises)
            {
                var exercise = Model.GetModel<Exercise>(exerciseId);
                base.CreateButton(exercise.UnityObjectName, exercise.Name);
            }
        }
    }
}
