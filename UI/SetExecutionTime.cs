namespace HSA.RehaGame.UI
{
    using DB.Models;
    using Manager;
    using UnityEngine;
    using UnityEngine.UI;

    public class SetExecutionTime : MonoBehaviour
    {
        public GameObject dbManagerPrefab;

        // Use this for initialization
        void Start()
        {
            string timeTable = "";
            var information = Model.GetModel<ExerciseInformation>("end").Order;

            var times = GameManager.ExecutionTimes;

            if (times != null)
            {
                foreach (var time in times)
                {
                    var executionTime = time.Value.TotalSeconds.ToString("00");

                    if (executionTime[0] == '0')
                        executionTime = executionTime.Substring(1);

                    timeTable += string.Format("{0}: {1} seconds\n", time.Key, executionTime);
                }

                this.GetComponent<Text>().text = string.Format(information, GameManager.ActiveExercise.Name, timeTable);
            }
        }
    }
}