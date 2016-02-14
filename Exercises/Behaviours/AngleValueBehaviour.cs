namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB;
    using DB.Models;
    using FulFillables;
    using Math;
    using UI.VisualExercise;
    using Windows.Kinect;

    public class AngleValueBehaviour : BaseJointValueBehaviour
    {
        private double tolerance;

        private double angle;
        private double minAngle;
        private double maxAngle;

        public AngleValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base(value, unityObjectName, activeJoint, passiveJoint, dbManager, settings, drawing, previous)
        {
            this.tolerance = settings.GetValue<int>("angleTolerance");
            this.angle = 0;
            this.minAngle = initialValue - tolerance;
            this.maxAngle = initialValue + tolerance;
        }

        public override bool IsFulfilled(Body body)
        {
            angle = Calculations.GetAngle(null); // ToDo Winkelberechnung

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
