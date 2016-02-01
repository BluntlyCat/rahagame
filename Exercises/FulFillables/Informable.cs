namespace HSA.RehaGame.Exercises.FulFillables
{
    using User;
    using Windows.Kinect;

    public abstract class Informable : FulFillable
    {
        protected string information = "";

        public abstract string Information();

        public abstract void VisualInformation(Body body);

        public abstract void Debug(Body body);
    }
}
