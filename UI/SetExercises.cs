namespace HSA.RehaGame.UI
{
    using DB.Models;
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
            var exercises = Model.All<Exercise>();
            bool isFirst = true;

            foreach (var name in exercises)
            {
                var button = Instantiate(exerciseButton) as Transform;
                var textComponent = button.GetComponentInChildren<Text>();
                var image = button.GetComponent<RawImage>();

                var exercise = Model.GetModel<Exercise>(name);
                // ToDo GameManager.AddExercise(exercise);

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