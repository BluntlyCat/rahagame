namespace HSA.RehaGame.Exercises.Actions
{
    using System;
    using User;
    using Windows.Kinect;

    public class HoldAction : BaseAction
    {
        private double start = -1;

        public HoldAction(string unityObjectName, double value) : base(unityObjectName, value)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            double now = DateTime.Now.TimeOfDay.TotalSeconds;

            if (start == -1)
                start = now;

            else
            {
                var timeDelta = now - start;
                value = (initialValue - timeDelta);

                isFulfilled = timeDelta >= initialValue;
            }

            return isFulfilled;
        }

        public override void VisualInformation(Body body)
        {
            
        }

        public override void Reset()
        {
            start = -1;
            value = initialValue;
            isFulfilled = false;
        }
    }
}
