namespace HSA.RehaGame.UI.Statistic.Views
{
    using DB.Models;
    using Manager;
    using System;
    public class DataOfDate : StatisticView
    {
        public DataOfDate(long date) : base ()
        {
            statisticData = StatisticData.GetByDate(GameManager.ActivePatient, date);

            this.statisticTitle = new DateTime(date).ToString("dd.MM.yyyy");
            this.commonCountText = GetFavoriteExercise().Name;
            this.doneCountText = StatisticData.ExercisesByDate(GameManager.ActivePatient, date, false).Count.ToString();
        }

        public override void Set()
        {
            base.Set();
        }
    }
}
