namespace HSA.RehaGame.Exercises.Actions
{
    using System;
    using UI.VisualExercise;
    using Windows.Kinect;

    public class RepeatAction : BaseAction
    {
        public RepeatAction(string unityObjectName, double value, Drawing drawing) : base(unityObjectName, value, drawing)
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
