namespace HSA.RehaGame.Exercises.Actions
{
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using System;
    using UI.Feedback;
    using Windows.Kinect;
    public class HoldAction : BaseAction
    {
        protected double start = -1;

        public HoldAction(string unityObjectName, double value, FulFillable previous, SettingsManager settingsManager, Feedback feedback, PitchType pitchType) : base(unityObjectName, value, previous, settingsManager, feedback, pitchType)
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

        public override void PlayValue()
        {
            feedback.PitchValue(base.pitchType, value);
        }

        public override void Reset()
        {
            start = -1;
            value = initialValue;
            isFulfilled = false;
        }
    }
}
