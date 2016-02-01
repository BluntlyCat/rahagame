namespace HSA.RehaGame.UI.VisualExercise
{
    using UnityEngine;

    public class ShowCircle : MonoBehaviour
    {
        public GameObject referenceCircleObject;
        public GameObject currentCircleObject;

        private Circle referenceCircle;
        private Circle currentCircle;

        void Start()
        {
            this.referenceCircle = referenceCircleObject.GetComponent<Circle>();
            this.currentCircle = currentCircleObject.GetComponent<Circle>();
        }

        public void UpdateCircles(float initialAngle, float currentAngle)
        {
            referenceCircle.UpdateCircle(initialAngle, initialAngle > currentAngle ? 1 : 0);
            currentCircle.UpdateCircle(currentAngle, initialAngle > currentAngle ? 0 : 1);
        }
    }
}
