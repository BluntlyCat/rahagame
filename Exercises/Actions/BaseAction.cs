namespace HSA.RehaGame.Exercises.Actions
{
    using System.Collections.Generic;
    using DB;
    using FulFillables;
    using UI = UI.VisualExercise;
    using User;
    using Windows.Kinect;
    using System;
    using InGame;

    public abstract class BaseAction : Drawable
    {
        protected double value;
        protected double initialValue;

        public BaseAction(string unityObjectName, double value, FulFillable previous, Database dbManager, Settings settings, UI.Drawing drawing) : base (dbManager, settings, drawing, previous)
        {
            this.initialValue = this.value = value;
            this.information = dbManager.GetExerciseInformation(unityObjectName, "action").GetValueFromLanguage("order");
        }

        public override void Draw(Body body)
        {
            
        }

        public override void Clear()
        {
            drawing.ClearDrawings();
        }

        public override void Write(Body body)
        {
            drawing.ShowInformation(string.Format(information, value.ToString("0")));
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
