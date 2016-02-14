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
            var information = Model.GetModel<ExerciseInformation>("end").Order;
            var time = GameManager.ExecutionTime.ToString("00");

            if (time[0] == '0')
                time = time.Substring(1);

            this.GetComponent<Text>().text = string.Format(information, time);
        }
    }
}