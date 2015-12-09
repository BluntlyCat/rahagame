namespace HSA.RehaGame.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class SetExerciseMenuHeaderText : MonoBehaviour
    {
        void Start()
        {
            var header = GameObject.FindGameObjectWithTag("ExerciseMenuHeader").GetComponent<Text>();
            header.text = Application.loadedLevelName;
        }
    }
}
