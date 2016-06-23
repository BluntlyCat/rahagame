namespace HSA.RehaGame.Exercises.Actions
{
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using UI.Feedback;

    public class WaitAction : HoldAction
    {
        public WaitAction(string unityObjectName, double value, FulFillable previous, SettingsManager settingsManager, Feedback feedback, PitchType pitchType) : base(unityObjectName, value, previous, settingsManager, feedback, pitchType)
        {

        }
    }
}
