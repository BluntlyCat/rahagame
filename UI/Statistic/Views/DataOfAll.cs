namespace HSA.RehaGame.UI.Statistic.Views
{
    using DB.Models;
    using Manager;

    public class DataOfAll : StatisticView
    {
        public DataOfAll() : base ()
        {
            statisticData = StatisticData.GetAll(GameManager.ActivePatient);
            var exercise = GetFavoriteExercise();
            this.commonCountText = exercise == null ? "-" : exercise.Name;
        }

        public override void Set()
        {
            base.Set();
        }
    }
}
