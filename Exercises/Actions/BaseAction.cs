namespace HSA.RehaGame.Exercises.Actions
{
    using System.Collections.Generic;
    using DB;
    using FulFillables;
    using Windows.Kinect;
    using Models = DB.Models;
    using UI = UI.VisualExercise;

    public abstract class BaseAction : Drawable
    {
        protected double value;
        protected double initialValue;

        public BaseAction(string unityObjectName, double value, FulFillable previous, Database dbManager, Models.Settings settings, UI.Drawing drawing) : base (dbManager, settings, drawing, previous)
        {
            this.initialValue = this.value = value;
            this.information = Models.Model.GetModel<Models.ExerciseInformation>(unityObjectName).Order;
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

        public override void Debug(Body body, IDictionary<string, Models.Joint> stressedJoints)
        {
            return;
        }

        public override void Debug(Body body, Models.Joint joint)
        {
            return;
        }

        public abstract void Reset();
    }
}
