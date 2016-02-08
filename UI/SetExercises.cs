namespace HSA.RehaGame.UI
{
    using DB;
    using Exercises;
    using InGame;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class SetExercises : MonoBehaviour
    {
        public GameObject dbManagerPrefab;
        public Transform exerciseButton;

        // Use this for initialization
        void Start()
        {
            Database dbManager = dbManagerPrefab.GetComponent<Database>();
            var table = dbManager.Query("editor_exercise", "SELECT unityObjectName FROM editor_exercise");
            bool isFirst = true;

            foreach (var row in table.Rows)
            {
                var button = Instantiate(exerciseButton) as Transform;
                var textComponent = button.GetComponentInChildren<Text>();
                var image = button.GetComponent<RawImage>();

                GameState.ActivePatient.ResetJoints();
                var exercise = new Exercise(row.GetValue("unityObjectName"), dbManager);
                GameState.AddExercise(exercise.Select() as Exercise);

                button.SetParent(this.transform, false);
                button.name = exercise.UnityObjectName;
                button.tag = "inputElement";
                textComponent.text = exercise.Name;
                image.texture = exercise.Thumbnail;

                if(isFirst)
                {
                    var eventSystem = GameObject.Find("EventSystem");
                    eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = image.gameObject;
                    isFirst = false;
                }
            }
        }
    }
}