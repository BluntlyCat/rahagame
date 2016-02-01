namespace HSA.RehaGame.Exercises.Actions
{
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;

    public abstract class BaseJointAction : BaseAction
    {
        protected PatientJoint baseJoint;

        public BaseJointAction(PatientJoint baseJoint, string unityObjectName, double value) : base(unityObjectName, value)
        {
            this.baseJoint = baseJoint;
        }

        public abstract override bool IsFulfilled(Body body);

        public abstract override void Reset();

        public override string Information()
        {
            return base.Information();
        }

        public override void Debug(Body body)
        {
            var joint = body.Joints[baseJoint.JointType];
            Drawing.DrawDebug(body, baseJoint);
        }

        public PatientJoint BaseJoint
        {
            get
            {
                return baseJoint;
            }
        }
    }
}
