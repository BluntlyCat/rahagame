namespace HSA.RehaGame.UI.Statistic.Orders
{
    using Manager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    public abstract class StatisticOrder
    {
        private GameObject orderButton;

        protected PatientManager patientManager;

        protected GameObject buttonPrefab;
        protected GameObject buttonStorage;

        public StatisticOrder(PatientManager patientManager, GameObject buttonPrefab, GameObject buttonStorage)
        {
            this.patientManager = patientManager;
            this.buttonPrefab = buttonPrefab;
            this.buttonStorage = buttonStorage;
        }

        protected void CreateButton(string buttonName, string buttonText)
        {
            var exerciseButton = UnityEngine.Object.Instantiate(buttonPrefab);

            exerciseButton.name = buttonName;
            exerciseButton.GetComponentInChildren<Text>().text = buttonText;
            exerciseButton.transform.SetParent(buttonStorage.transform, false);
        }
    }
}
