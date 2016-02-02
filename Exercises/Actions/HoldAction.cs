namespace HSA.RehaGame.Exercises.Actions
{
    using System;
    using FulFillables;
    using Windows.Kinect;

    public class HoldAction : BaseAction
    {
        protected double start = -1;

        public HoldAction(string unityObjectName, double value, FulFillable previous) : base(unityObjectName, value, previous)
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

        public override void Reset()
        {
            start = -1;
            value = initialValue;
            isFulfilled = false;
        }
    }
}
