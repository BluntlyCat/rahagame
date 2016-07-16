namespace HSA.RehaGame.Exercises.FulFillables
{
    using DB.Models;
    using Manager;
    using Windows.Kinect;
    public class StepGroup : BaseStep
    {
        public StepGroup(string description, StatisticType statisticType, PatientJoint affectedJoint, BaseStep previous, int repetitions, WriteStatisticManager statisticManager) : base(description, statisticType, affectedJoint, previous, repetitions, statisticManager) {}

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
