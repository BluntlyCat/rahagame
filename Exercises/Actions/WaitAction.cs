namespace HSA.RehaGame.Exercises.Actions
{
    using DB.Models;
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using UI.Feedback;

    public class WaitAction : HoldAction
    {
        public WaitAction(string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, double value, FulFillable previous, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, int repetitions, WriteStatisticManager statisticManager) : base(unityObjectName, statisticType, affectedJoint, value, previous, settingsManager, feedback, pitchType, repetitions, statisticManager)
        {

        }
    }
}
