namespace HSA.RehaGame.UI
{
    using InGame;
    using UnityEngine;
    using UnityEngine.UI;

    public class SetExerciseMenuHeaderText : MonoBehaviour
    {
        void Start()
        {
            var header = GameObject.Find("ExerciseHeader").GetComponent<Text>();
        }
    }
}
