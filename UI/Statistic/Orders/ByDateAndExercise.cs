namespace HSA.RehaGame.UI.Statistic
{
    using Manager;
    using Orders;
    using UnityEngine;
    using Views;
    public class ByDateAndExercise : MonoBehaviour
    {
        private GameObject gameManager;
        private SceneManager sceneManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        public void OrderByDateAndExercise()
        {
            GameManager.StatisticViewData.StatisticViewType = StatisticViewTypes.bySelectedDate;
            GameManager.StatisticViewData.StatisticOrder = StatisticOrders.exerciseByDate;
            GameManager.StatisticViewData.FilterValue = this.name;
            sceneManager.LoadStatisticMenu();
        }
    }
}
