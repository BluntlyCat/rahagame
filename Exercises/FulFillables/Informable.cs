namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;

    public abstract class Informable : FulFillable
    {
        protected Drawing dwarable;

        public Informable(FulFillable previous) : base(previous)
        {

        }

        protected string information = "";

        public abstract string Information();

        public abstract void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints);

        public abstract void Debug(Body body, PatientJoint jointJoint);
    }
}
