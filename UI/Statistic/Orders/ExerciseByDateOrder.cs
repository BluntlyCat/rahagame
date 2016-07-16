namespace HSA.RehaGame.UI.Statistic.Orders
{
    using DB.Models;
    using Manager;
    using UnityEngine;
    public class ExerciseByDateOrder : StatisticOrder
    {
        public ExerciseByDateOrder(PatientManager patientManager, long date, GameObject buttonPrefab, GameObject buttonStorage) : base(patientManager, buttonPrefab, buttonStorage)
        {
            var exercises = StatisticData.ExercisesByDate(GameManager.ActivePatient, date);

            foreach (var exerciseId in exercises)
            {
                var exercise = Model.GetModel<Exercise>(exerciseId);
                base.CreateButton(exercise.UnityObjectName, exercise.Name);
            }
        }
    }
}
