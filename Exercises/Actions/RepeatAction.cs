namespace HSA.RehaGame.Exercises.Actions
{
    using System;
    using FulFillables;
    using UI.VisualExercise;
    using Windows.Kinect;

    public class RepeatAction : BaseAction
    {
        public RepeatAction(string unityObjectName, double value, FulFillable previous) : base(unityObjectName, value, previous)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = value == 0;

            return isFulfilled;
        }

        public override void Reset()
        {
            isFulfilled = false;
        }
    }
}
