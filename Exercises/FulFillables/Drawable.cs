namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using DB;
    using InGame;
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;

    public abstract class Drawable : FulFillable
    {
        protected string information = "";

        protected Database dbManager;
        protected Settings settings;
        protected Drawing drawing;

        public Drawable(Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base (previous)
        {
            this.dbManager = dbManager;
            this.drawing = drawing;
        }

        public abstract void Write(Body body);

        public abstract void Draw(Body body);

        public abstract void Clear();

        public abstract void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints);

        public abstract void Debug(Body body, PatientJoint jointJoint);
    }
}
