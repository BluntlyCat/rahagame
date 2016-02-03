namespace HSA.RehaGame.UI
{
    using DB;
    using InGame;
    using UnityEngine;
    using UnityEngine.UI;

    public class SetExecutionTime : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            var information = DBManager.GetExerciseInformation("executionTime", "end").GetValueFromLanguage("order");
            var time = GameState.ExecutionTime.ToString("00");

            if (time[0] == '0')
                time = time.Substring(1);

            this.GetComponent<Text>().text = string.Format(information, time);
        }
    }
}