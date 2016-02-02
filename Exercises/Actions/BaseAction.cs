namespace HSA.RehaGame.Exercises.Actions
{
    using System.Collections.Generic;
    using DB;
    using FulFillables;
    using User;
    using UI.VisualExercise;
    using Windows.Kinect;
    using System;

    public abstract class BaseAction : Informable
    {
        protected double value;
        protected double initialValue;

        public BaseAction(string unityObjectName, double value, FulFillable previous) : base (previous)
        {
            this.initialValue = this.value = value;
            this.information = DBManager.GetExerciseInformation(unityObjectName, "action").GetValueFromLanguage("order");
        }

        public override string Information()
        {
            return string.Format(information, value.ToString("0"));
        }

        public override void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            return;
        }

        public override void Debug(Body body, PatientJoint jointJoint)
        {
            return;
        }

        public abstract void Reset();
    }
}
