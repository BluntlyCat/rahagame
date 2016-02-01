namespace HSA.RehaGame.Exercises.Actions
{
    using System;
    using Windows.Kinect;

    public class RepeatAction : BaseAction
    {
        public RepeatAction(string unityObjectName, double value) : base(unityObjectName, value)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = value == 0;

            return isFulfilled;
        }

        public override void VisualInformation(Body body)
        {
            
        }

        public override void Reset()
        {
            isFulfilled = false;
        }
    }
}
