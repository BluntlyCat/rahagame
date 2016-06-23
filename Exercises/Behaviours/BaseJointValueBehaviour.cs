namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB.Models;
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using UI.Feedback;
    using Windows.Kinect;

    public abstract class BaseJointValueBehaviour : BaseJointBehaviour
    {
        protected double value;
        protected double initialValue;

        public abstract override bool IsFulfilled(Body body);

        public BaseJointValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous) : base(unityObjectName, activeJoint, passiveJoint, settingsManager, feedback, pitchType, previous)
        {
            this.initialValue = this.value = value;
        }

        public override void Write(Body body)
        {
            feedback.ShowInformation(string.Format(information, value.ToString("0")));
        }
    }
}
