namespace HSA.RehaGame.Manager
{
    using DB.Models;
    using UI.Statistic.Orders;
    using UI.Statistic.Views;
    using UnityEngine;
    using UnityEngine.UI;
    public class ReadStatisticManager : MonoBehaviour
    {
        public GameObject gameManagerPrefab;

        public GameObject statisticTitlePrefab;

        public GameObject doneCountLabelPrefab;
        public GameObject doneCountTextPrefab;

        public GameObject commonCoundLabelPrefab;
        public GameObject commonCountTextPrefab;

        public GameObject cancelationRateLabelPrefab;
        public GameObject cancelationRateImagePrefab;
        public GameObject cancelationRateAmountPrefab;

        public GameObject orderByExerciseButtonPrefab;
        public GameObject orderByDateButtonPrefab;

        public GameObject buttonStoragePrefab;

        public GameObject statisticByExerciseButtonPrefab;
        public GameObject statisticByDateButtonPrefab;

        public Color activeOrderButtonColor;

        private PatientManager patientManager;

        private Text statisticTitle;

        private Text doneCountLabel;
        private Text doneCountText;

        private Text commonCountLabel;
        private Text commonCountText;

        private Image cancelationRateImage;
        private Text cancelationRateLabel;
        private Text cancelationRateText;

        private Text ordereByExerciseButtonText;
        private Text orderByDateButtonText;

        private StatisticView statisticView;

        private ColorBlock initialOrderButtonColors;

        void Start()
        {
            patientManager = gameManagerPrefab.GetComponentInChildren<PatientManager>();

            statisticTitle = statisticTitlePrefab.GetComponent<Text>();

            doneCountLabel = doneCountLabelPrefab.GetComponent<Text>();
            doneCountLabel.text = string.Format("{0}:", Model.GetModel<ValueTranslation>("countDoneExercises").Translation);

            doneCountText = doneCountTextPrefab.GetComponent<Text>();

            commonCountLabel = commonCoundLabelPrefab.GetComponent<Text>();
            commonCountLabel.text = string.Format("{0}:", Model.GetModel<ValueTranslation>("favoriteExercise").Translation);

            commonCountText = commonCountTextPrefab.GetComponent<Text>();

            cancelationRateLabel = cancelationRateLabelPrefab.GetComponent<Text>();
            cancelationRateLabel.text = string.Format("{0}:", Model.GetModel<ValueTranslation>("cancelationRate").Translation);

            ordereByExerciseButtonText = orderByExerciseButtonPrefab.GetComponentInChildren<Text>();
            ordereByExerciseButtonText.text = string.Format("{0}", Model.GetModel<ValueTranslation>("allByExercise").Translation);

            orderByDateButtonText = orderByDateButtonPrefab.GetComponentInChildren<Text>();
            orderByDateButtonText.text = string.Format("{0}", Model.GetModel<ValueTranslation>("allByDate").Translation);

            cancelationRateImage = cancelationRateImagePrefab.GetComponent<Image>();

            cancelationRateText = cancelationRateAmountPrefab.GetComponent<Text>();

            GetOrder();
            statisticView = GetView();

            statisticView.Set();

            statisticTitle.text = statisticView.StatisticTitle;
            doneCountText.text = statisticView.DoneCountText;
            commonCountText.text = statisticView.CommonCountText;

            cancelationRateImage.fillAmount = statisticView.CancelationRatePerCent / 100;
            cancelationRateText.text = string.Format("{0} %", statisticView.CancelationRatePerCent.ToString("0.00"));
        }

        private void SetColorOrderButton(GameObject orderButtonPrefab)
        {
            if (GameManager.StatisticViewData.OrderButton)
            {
                GameManager.StatisticViewData.OrderButton.colors = initialOrderButtonColors;
                GameManager.StatisticViewData.OrderButton = orderButtonPrefab.GetComponent<Button>();
            }
            else
            {
                GameManager.StatisticViewData.OrderButton = orderButtonPrefab.GetComponent<Button>();
                this.initialOrderButtonColors = GameManager.StatisticViewData.OrderButton.colors;
            }

            var colorBlock = GameManager.StatisticViewData.OrderButton.colors;
            colorBlock.normalColor = activeOrderButtonColor;

            GameManager.StatisticViewData.OrderButton.colors = colorBlock;
        }

        private StatisticOrder GetOrder()
        {
            switch(GameManager.StatisticViewData.StatisticOrder)
            {
                case StatisticOrders.no:
                    return null;

                case StatisticOrders.exercise:
                    this.SetColorOrderButton(orderByExerciseButtonPrefab);
                    return new ExerciseOrder(patientManager, statisticByExerciseButtonPrefab, buttonStoragePrefab);

                case StatisticOrders.date:
                    this.SetColorOrderButton(orderByDateButtonPrefab);
                    return new DateOrder(patientManager, statisticByDateButtonPrefab, buttonStoragePrefab);

                case StatisticOrders.exerciseByDate:
                    this.SetColorOrderButton(orderByDateButtonPrefab);
                    return new ExerciseByDateOrder(patientManager, long.Parse(GameManager.StatisticViewData.FilterValue.ToString()), statisticByExerciseButtonPrefab, buttonStoragePrefab);

                default:
                    return new ExerciseOrder(patientManager, statisticByExerciseButtonPrefab, buttonStoragePrefab);
            }
        }

        private StatisticView GetView()
        {
            switch (GameManager.StatisticViewData.StatisticViewType)
            {
                case StatisticViewTypes.allByExercise:
                    return new DataOfAll();

                case StatisticViewTypes.allByDate:
                    return new DataOfAll();

                case StatisticViewTypes.bySelectedExercise:
                    var exercise = Model.GetModel<Exercise>(GameManager.StatisticViewData.FilterValue);
                    return new DataOfExercise(exercise);

                case StatisticViewTypes.bySelectedDate:
                    return new DataOfDate(long.Parse(GameManager.StatisticViewData.FilterValue.ToString()));

                case StatisticViewTypes.byCurrentExercise:
                    return new DataOfCurrentExercise();

                default:
                    return new DataOfAll();
            }
        }
    }
}
