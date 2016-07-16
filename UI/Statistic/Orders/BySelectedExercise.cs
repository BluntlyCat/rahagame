namespace HSA.RehaGame.UI.Statistic
{
    using Manager;
    using Orders;
    using UnityEngine;
    using Views;
    public class BySelectedExercise : MonoBehaviour
    {
        private GameObject gameManager;
        private SceneManager sceneManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        public void OrderBySelectedExercise()
        {
            GameManager.StatisticViewData.FilterValue = this.name;
            GameManager.StatisticViewData.StatisticViewType = StatisticViewTypes.bySelectedExercise;
            GameManager.StatisticViewData.StatisticOrder = StatisticOrders.no;
            sceneManager.LoadStatisticMenu();
        }
    }
}
