namespace HSA.RehaGame.UI.Statistic
{
    using Manager;
    using Orders;
    using UnityEngine;
    using Views;
    public class ByDate : MonoBehaviour
    {
        private GameObject gameManager;
        private SceneManager sceneManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        public void OrderByDate()
        {
            GameManager.StatisticViewData.StatisticViewType = StatisticViewTypes.allByDate;
            GameManager.StatisticViewData.StatisticOrder = StatisticOrders.date;
            sceneManager.LoadStatisticMenu();
        }
    }
}
