namespace HSA.RehaGame.UI.VisualExercise
{
    public class ShowCircle
    {
        private Circle referenceCircle;
        private Circle currentCircle;

        public ShowCircle(Circle reference, Circle current)
        {
            this.referenceCircle = reference;
            this.currentCircle = current;
        }

        public void Update(float initialAngle, float currentAngle)
        {
            referenceCircle.Update(initialAngle, initialAngle > currentAngle ? 1 : 0);
            currentCircle.Update(currentAngle, initialAngle > currentAngle ? 0 : 1);
        }
    }
}
