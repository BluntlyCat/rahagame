namespace HSA.RehaGame.Exercises.FulFillables
{
    using DB;
    using Windows.Kinect;

    public class StepGroup : BaseStep
    {
        public StepGroup(string description, BaseStep previous, Database dbManager) : base(description, dbManager, previous) {}

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
