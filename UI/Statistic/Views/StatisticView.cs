namespace HSA.RehaGame.UI.Statistic.Views
{
    using DB.Models;
    using Manager;
    using System.Collections.Generic;

    public abstract class StatisticView
    {
        protected IList<StatisticData> statisticData = new List<StatisticData>();
        protected IList<StatisticData> finishedStatisticData = new List<StatisticData>();
        protected IList<StatisticData> canceledStatisticData = new List<StatisticData>();
        protected IList<StatisticData> unfulfilledStatisticData = new List<StatisticData>();

        protected string statisticTitle;

        protected string doneCountLabel;
        protected string doneCountText;

        protected string commonCountLabel;
        protected string commonCountText;

        protected float cancelationRatePerCent = 0;

        public StatisticView()
        {
            this.statisticTitle = GameManager.ActivePatient.Name;
            this.doneCountText = StatisticData.Exercises(GameManager.ActivePatient, false).Count.ToString();
        }

        protected Exercise GetFavoriteExercise()
        {
            int exerciseCounter = 0;
            Exercise mostCountedExercise = null;

            IDictionary<Exercise, int> doneExercises = new Dictionary<Exercise, int>();

            foreach (var data in statisticData)
            {
                Exercise exercise = data.Exercise;

                if (doneExercises.ContainsKey(exercise))
                    doneExercises[exercise]++;
                else
                    doneExercises.Add(exercise, 1);
            }

            foreach (var exercise in doneExercises)
            {
                if (exercise.Value > exerciseCounter)
                {
                    exerciseCounter = exercise.Value;
                    mostCountedExercise = exercise.Key;
                }
            }

            return mostCountedExercise;
        }

        protected int CountExercises()
        {
            int count = 0;

            foreach (var data in statisticData)
            {
                if (data.DataType == StatisticType.exercise)
                    count++;
            }

            return count;
        }

        public virtual void Set()
        {
            foreach (var statistic in statisticData)
            {
                switch (statistic.State)
                {
                    case StatisticStates.finished:
                        finishedStatisticData.Add(statistic);
                        break;

                    case StatisticStates.canceled:
                        canceledStatisticData.Add(statistic);
                        break;

                    case StatisticStates.unfulfilled:
                        unfulfilledStatisticData.Add(statistic);
                        break;
                }
            }

            if (statisticData.Count == 0)
                cancelationRatePerCent = 0f;

            else
                cancelationRatePerCent = (100f * (float)canceledStatisticData.Count) / (float)statisticData.Count;
        }

        public float CancelationRatePerCent
        {
            get
            {
                return cancelationRatePerCent;
            }
        }

        public int SuccessfulStatisticCount
        {
            get
            {
                return finishedStatisticData.Count;
            }
        }

        public int CanceledStatisticCount
        {
            get
            {
                return canceledStatisticData.Count;
            }
        }

        public string StatisticTitle
        {
            get
            {
                return statisticTitle == null ? "-" : statisticTitle;
            }
        }

        public string DoneCountText
        {
            get
            {
                return doneCountText == null ? "-" : doneCountText;
            }
        }

        public string CommonCountText
        {
            get
            {
                return commonCountText == null ? "-" : commonCountText;
            }
        }
    }
}
