namespace HSA.RehaGame.Exercises.Behaviours
{
    using FulFillables;
    using DB;
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;
    using InGame;

    public abstract class BaseJointValueBehaviour : BaseJointBehaviour
    {
        protected double value;
        protected double initialValue;

        public abstract override bool IsFulfilled(Body body);

        public BaseJointValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base(unityObjectName, activeJoint, passiveJoint, dbManager, settings, drawing, previous)
        {
            this.initialValue = this.value = value;
        }

        public override void Write(Body body)
        {
            drawing.ShowInformation(string.Format(information, value.ToString("0")));
        }
    }
}
