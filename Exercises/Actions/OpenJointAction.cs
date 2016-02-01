namespace HSA.RehaGame.Exercises.Actions
{
    using User.Kinect;
    using InGame;
    using Math;
    using User;
    using Windows.Kinect;
    using System;
    using UI.VisualExercise;
    using UnityEngine;

    public class OpenJointAction : BaseJointAction
    {
        private double tolerance;

        private double angle;
        private double minAngle;
        private double maxAngle;

        public OpenJointAction(string unityObjectName, PatientJoint baseJoint, double value, Drawing drawing) : base(baseJoint, unityObjectName, value, drawing)
        {
            this.tolerance = RGSettings.angleTolerance;

            this.angle = 0;
            this.minAngle = initialValue - tolerance;
            this.maxAngle = initialValue + tolerance;
        }

        public override bool IsFulfilled(Body body)
        {
            angle = Calculations.GetAngle(KinectJoint.GetJoints(body, baseJoint));

            isFulfilled = minAngle <= angle && angle <= maxAngle;

            return isFulfilled;
        }

        public override string Information()
        {
            return string.Format(information, baseJoint.Translation, value.ToString("000"));
        }

        public override void VisualInformation(Body body)
        {
            drawing.DrawCircle(body, baseJoint, initialValue - tolerance, angle);
        }

        public override void Reset()
        {
            value = initialValue;
            isFulfilled = false;
        }
    }
}
