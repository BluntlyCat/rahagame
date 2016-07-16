namespace HSA.RehaGame.UI.Statistic.Views
{
    using DB.Models;
    using Manager;
    using System.Collections.Generic;
    public class DataOfExercise : StatisticView
    {
        public DataOfExercise(Exercise exercise) : base ()
        {
            statisticData = StatisticData.GetByExercise(GameManager.ActivePatient, exercise);
            this.statisticTitle = exercise.Name;
            this.doneCountText = this.CountExercises().ToString();
            this.commonCountText = GetFavoriteExercise().Name;
        }

        public override void Set()
        {
            base.Set();
        }
    }
}
