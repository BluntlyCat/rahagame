namespace HSA.RehaGame.UI.Statistic
{
    using Manager;
    using Orders;
    using UnityEngine;
    using Views;
    public class ByExercise : MonoBehaviour
    {
        private GameObject gameManager;
        private SceneManager sceneManager;

        void Start()
        {
            gameManager = GameObject.Find("GameManager");
            sceneManager = gameManager.GetComponent<SceneManager>();
        }

        public void OrderByExercise()
        {
            GameManager.StatisticViewData.StatisticViewType = StatisticViewTypes.allByExercise;
            GameManager.StatisticViewData.StatisticOrder = StatisticOrders.exercise;
            sceneManager.LoadStatisticMenu();
        }
    }
}
