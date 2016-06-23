namespace HSA.RehaGame.Exercises.Actions
{
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using UI.Feedback;
    using Windows.Kinect;

    public class RepeatAction : BaseAction
    {
        public RepeatAction(string unityObjectName, double value, FulFillable previous, SettingsManager settingsManager, Feedback feedback, PitchType pitchType) : base(unityObjectName, value, previous, settingsManager, feedback, pitchType)
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
