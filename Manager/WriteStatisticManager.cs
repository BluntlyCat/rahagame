namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using Mono.Data.Sqlite;
    using System;
    using UnityEngine;
    public class WriteStatisticManager : MonoBehaviour
    {
        public GameObject gameManagerPrefab;

        private long date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Ticks;

        void Start()
        {
        }

        public StatisticData AddStatistic(string name, StatisticType dataType, StatisticStates state, string message, PatientJoint affectedJoint)
        {
            var statistic = new StatisticData(date, name, dataType, state, message, affectedJoint, GameManager.ActiveExercise, GameManager.ActivePatient);
            GameManager.ActivePatient.CurrentStatistic.Add(statistic);

            return statistic;
        }

        public void SaveStatistic()
        {
            foreach (var statisticData in GameManager.ActivePatient.CurrentStatistic)
            {
                var result = statisticData.Save();

                if (result.ErrorCode == SQLiteErrorCode.Ok)
                {
                    GameManager.ActivePatient.AddManyToManyRelation(GameManager.ActivePatient.GetPropertyInfo("StatisticData"), statisticData);
                    GameManager.ActivePatient.StatisticData.Add(statisticData.Id, statisticData);
                }
            }
        }
    }
}
