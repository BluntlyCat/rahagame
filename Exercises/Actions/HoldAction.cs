namespace HSA.RehaGame.Exercises.Actions
{
    using System;
    using FulFillables;
    using DB;
    using UI.VisualExercise;
    using Windows.Kinect;
    using InGame;

    public class HoldAction : BaseAction
    {
        protected double start = -1;

        public HoldAction(string unityObjectName, double value, FulFillable previous, Database dbManager, Settings settings, Drawing drawing) : base(unityObjectName, value, previous, dbManager, settings, drawing)
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
