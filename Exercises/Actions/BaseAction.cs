namespace HSA.RehaGame.Exercises.Actions
{
    using DB.Models;
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using System;
    using System.Collections.Generic;
    using UI.Feedback;
    using Windows.Kinect;
    using Models = DB.Models;

    public abstract class BaseAction : Informable
    {
        protected double value;
        protected double initialValue;

        public BaseAction(string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, double value, FulFillable previous, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, int repetitions, WriteStatisticManager statisticManager) : base (settingsManager, feedback, pitchType, unityObjectName, statisticType, affectedJoint, previous, repetitions, statisticManager)
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
    }
}
