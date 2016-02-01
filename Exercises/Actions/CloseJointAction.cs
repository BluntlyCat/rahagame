namespace HSA.RehaGame.Exercises.Actions
{
    using InGame;
    using Math;
    using User;
    using Windows.Kinect;
    using User.Kinect;
    using System;
    using UI.VisualExercise;

    public class CloseJointAction : BaseJointAction
    {
        private double tolerance;

        private double angle;
        private double minAngle;
        private double maxAngle;

        public CloseJointAction(string unityObjectName, PatientJoint baseJoint, double value) : base(baseJoint, unityObjectName, value)
        {
            this.tolerance = RGSettings.angleTolerance;

            this.angle = 0;
            this.minAngle = initialValue - tolerance;
            this.maxAngle = initialValue + tolerance;
        }

        public override bool IsFulfilled(Body body)
        {
            angle = Calculations.GetAngle(KinectJoint.GetJoints(body, baseJoint));

            isFulfilled = angle >= minAngle && angle <= maxAngle;

            return isFulfilled;
        }

        public override string Information()
        {
            return string.Format(information, baseJoint.Translation, value.ToString("000"));
        }

        public override void VisualInformation(Body body)
        {
            Drawing.DrawCircle(body, baseJoint, initialValue, angle);
        }

        public override void Reset()
        {
            value = initialValue;
            isFulfilled = false;
        }
    }
}
