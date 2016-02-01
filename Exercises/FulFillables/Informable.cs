namespace HSA.RehaGame.Exercises.FulFillables
{
    using UI.VisualExercise;
    using Windows.Kinect;

    public abstract class Informable : FulFillable
    {
        protected Drawing drawing;

        public Informable(Drawing drawing)
        {
            this.drawing = drawing;
        }

        protected string information = "";

        public abstract string Information();

        public abstract void VisualInformation(Body body);

        public abstract void Debug(Body body);
    }
}
