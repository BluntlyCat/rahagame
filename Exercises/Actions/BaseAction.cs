namespace HSA.RehaGame.Exercises.Actions
{
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using System.Collections.Generic;
    using UI.Feedback;
    using Windows.Kinect;
    using Models = DB.Models;
    public abstract class BaseAction : Informable
    {
        protected double value;
        protected double initialValue;

        public BaseAction(string unityObjectName, double value, FulFillable previous, SettingsManager settingsManager, Feedback feedback, PitchType pitchType) : base (settingsManager, feedback, pitchType, unityObjectName, previous)
        {
            this.type = Types.action;
            this.initialValue = this.value = value;
            this.information = Models.Model.GetModel<Models.ExerciseInformation>(unityObjectName).Order;
        }

        public override void Draw(Body body)
        {
            
        }

        public override void PlayFullfilledSound()
        {
            feedback.PitchFullfilledSound();
        }

        public override void Clear()
        {
            feedback.ClearDrawings();
        }

        public override void Write(Body body)
        {
            feedback.ShowInformation(string.Format(information, value.ToString("0")));
        }

        public override void PlayValue()
        {
            feedback.PitchValue(base.pitchType, value);
        }

        public override void Debug(Body body, IDictionary<string, Models.KinectJoint> stressedJoints)
        {
            return;
        }

        public override void Debug(Body body, Models.KinectJoint joint)
        {
            return;
        }

        public abstract void Reset();
    }
}
