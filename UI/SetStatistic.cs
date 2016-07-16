namespace HSA.RehaGame.UI
{
    using DB.Models;
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    public class SetStatistic : MonoBehaviour
    {
        public GameObject statisticTitleObject;
        public GameObject statisticTextObject;

        private Text statisticTitle;
        private Text statisticText;

        private int depth = 0;
        private string timeTable = "\n";

        // Use this for initialization
        void Start()
        {
            var information = Model.GetModel<ExerciseInformation>("end").Order;

            statisticTitle = statisticTitleObject.GetComponent<Text>();
            statisticText = statisticTextObject.GetComponent<Text>();

            statisticTitle.text = string.Format(information, GameManager.ActiveExercise.Name);

            if (GameManager.StatisticViewData.Data != null)
            {
                foreach (var statisticData in GameManager.StatisticViewData.Data)
                {
                }
            }

            statisticText.text = timeTable;
        }
    }
}