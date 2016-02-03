namespace HSA.RehaGame.Exercises.Behaviours
{
    using FulFillables;
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;

    public abstract class BaseJointValueBehaviour : BaseJointBehaviour
    {
        protected double value;
        protected double initialValue;

        public abstract override bool IsFulfilled(Body body);

        public BaseJointValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Drawing drawing, FulFillable previous) : base(unityObjectName, activeJoint, passiveJoint, drawing, previous)
        {
            this.initialValue = this.value = value;
        }

        public override void Write(Body body)
        {
            drawing.ShowInformation(string.Format(information, value.ToString("0")));
        }
    }
}
