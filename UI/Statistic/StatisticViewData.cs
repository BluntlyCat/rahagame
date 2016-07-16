namespace HSA.RehaGame.UI.Statistic
{
    using DB.Models;
    using HSA.RehaGame.UI.Statistic.Orders;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using Views;
    public class StatisticViewData
    {
        private static StatisticViewData viewData;

        private StatisticOrders statisticOrder;
        private StatisticViewTypes statisticViewType;

        private IList<StatisticData> statisticData;

        private Button orderButton;
        private object primaryKey;

        private StatisticViewData()
        {
            this.statisticOrder = StatisticOrders.exercise;
            this.statisticViewType = StatisticViewTypes.allByExercise;

            this.statisticData = new List<StatisticData>();

            this.orderButton = null;
            this.primaryKey = null;
        }

        public static StatisticViewData Instance
        {
            get
            {
                if (viewData == null)
                    viewData = new StatisticViewData();

                return viewData;
            }
        }

        public StatisticViewTypes StatisticViewType
        {
            get
            {
                return this.statisticViewType;
            }
            set
            {
                this.statisticViewType = value;
            }
        }

        public StatisticOrders StatisticOrder
        {
            get
            {
                return this.statisticOrder;
            }
            set
            {
                this.statisticOrder = value;
            }
        }

        public IList<StatisticData> Data
        {
            get
            {
                return this.statisticData;
            }
            set
            {
                this.statisticData = value;
            }
        }

        public Button OrderButton
        {
            get
            {
                return this.orderButton;
            }
            set
            {
                this.orderButton = value;
            }
        }

        public object FilterValue
        {
            get
            {
                return this.primaryKey;
            }
            set
            {
                this.primaryKey = value;
            }
        }
    }
}
