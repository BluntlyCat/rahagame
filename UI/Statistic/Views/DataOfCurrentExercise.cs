namespace HSA.RehaGame.UI.Statistic.Views
{
    using DB.Models;
    using Manager;
    public class DataOfCurrentExercise : StatisticView
    {
        public DataOfCurrentExercise() : base ()
        {
            statisticData = GameManager.StatisticViewData.Data;
            this.statisticTitle = GameManager.ActiveExercise.Name;
            this.doneCountText = this.CountExercises().ToString();
            this.commonCountText = GetFavoriteExercise().Name;
        }

        public override void Set()
        {
            base.Set();
        }
    }
}
