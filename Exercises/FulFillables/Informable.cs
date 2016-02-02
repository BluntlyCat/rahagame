namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using User;
    using Windows.Kinect;

    public abstract class Informable : FulFillable
    {
        public Informable(FulFillable previous) : base(previous)
        {

        }

        protected string information = "";

        public abstract string Information();

        public abstract void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints);

        public abstract void Debug(Body body, PatientJoint jointJoint);
    }
}
