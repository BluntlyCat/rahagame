namespace HSA.RehaGame.UI.Statistic.Orders
{
    using DB.Models;
    using Manager;
    using System;
    using UnityEngine;
    public class DateOrder : StatisticOrder
    {
        public DateOrder(PatientManager patientManager, GameObject buttonPrefab, GameObject buttonStorage) : base(patientManager, buttonPrefab, buttonStorage)
        {
            var ticks = StatisticData.Dates(GameManager.ActivePatient);

            foreach (var tick in ticks)
            {
                var date = new DateTime(tick);

                base.CreateButton(tick.ToString(), date.ToString("dd.MM.yyyy"));
            }
        }
    }
}
