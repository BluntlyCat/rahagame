namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB;
    using FulFillables;
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;

    public abstract class BaseJointBehaviour : Informable
    {
        private string unityObjectName;
        protected PatientJoint activeJoint;
        protected PatientJoint passiveJoint;

        public BaseJointBehaviour(string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Drawing drawing) : base(drawing)
        {
            this.unityObjectName = unityObjectName;
            this.activeJoint = activeJoint;
            this.passiveJoint = passiveJoint;
            this.information = DBManager.GetBehaviour(unityObjectName).GetValueFromLanguage("order");
        }

        public abstract override bool IsFulfilled(Body body);

        public override string Information()
        {
            return string.Format(information, activeJoint.Translation, passiveJoint.Translation);
        }

        public override void Debug(Body body)
        {
            drawing.DrawDebug(body, activeJoint);
        }

        public PatientJoint ActiveJoint
        {
            get
            {
                return activeJoint;
            }
        }

        public PatientJoint PassiveJoint
        {
            get
            {
                return passiveJoint;
            }
        }
    }
}
