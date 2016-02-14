namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB;
    using DB.Models;
    using FulFillables;
    using UI.VisualExercise;
    using Windows.Kinect;

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
