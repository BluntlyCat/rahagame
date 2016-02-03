namespace HSA.RehaGame.Exercises.Behaviours
{
    using FulFillables;
    using InGame;
    using Math;
    using UI.VisualExercise;
    using User;
    using User.Kinect;
    using Windows.Kinect;

    public class AngleValueBehaviour : BaseJointValueBehaviour
    {
        private double tolerance;

        private double angle;
        private double minAngle;
        private double maxAngle;

        public AngleValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Drawing drawing, FulFillable previous) : base(value, unityObjectName, activeJoint, passiveJoint, drawing, previous)
        {
            this.tolerance = RGSettings.angleTolerance;
            this.angle = 0;
            this.minAngle = initialValue - tolerance;
            this.maxAngle = initialValue + tolerance;
        }

        public override bool IsFulfilled(Body body)
        {
            angle = Calculations.GetAngle(KinectJoint.GetJoints(body, activeJoint));

            isFulfilled = minAngle <= angle && angle <= maxAngle;

            return isFulfilled;
        }

        public override void Write(Body body)
        {
            var angleString = value.ToString("000");

            if (angleString[0] == '0')
                angleString = angleString.Substring(1);

            drawing.ShowInformation(string.Format(information, activeJoint.Translation, angleString));
        }

        public override void Draw(Body body)
        {
            drawing.DrawCircle(body, activeJoint, initialValue - tolerance, angle);
        }

        public override void Clear()
        {
            drawing.ClearDrawings();
        }
    }
}
