namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using User;
    using Windows.Kinect;

    public class StepGroup : BaseStep
    {
        public StepGroup(string description, BaseStep previous) : base(description, previous) {}

        public override bool IsFulfilled(Body body)
        {
            return isFulfilled;
        }

        public override string Information()
        {
            return "";
        }

        public override void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            
        }

        public override void Debug(Body body, PatientJoint joint)
        {
            
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
