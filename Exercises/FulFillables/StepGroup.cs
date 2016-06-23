namespace HSA.RehaGame.Exercises.FulFillables
{
    using Windows.Kinect;

    public class StepGroup : BaseStep
    {
        public StepGroup(string description, BaseStep previous) : base(description, previous) {}

        public override bool IsFulfilled(Body body)
        {
            return isFulfilled;
        }

        public Step CurrentStep
        {
            get
            {
                return null;
            }
        }
    }
}
