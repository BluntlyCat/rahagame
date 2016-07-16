namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB.Models;
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using Math;
    using UI.Feedback;
    using Windows.Kinect;

    public class AngleValueBehaviour : BaseJointValueBehaviour
    {
        private double tolerance;

        private double angle;
        private double minAngle;
        private double maxAngle;

        public AngleValueBehaviour(double value, string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, PatientJoint activeJoint, PatientJoint childJoint, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base(value, unityObjectName, statisticType, affectedJoint, activeJoint, childJoint, settingsManager, feedback, pitchType, previous, repetitions, statisticManager)
        {
            this.tolerance = settingsManager.GetValue<int>("ingame", "angleTolerance");
            this.angle = 0;
            this.minAngle = initialValue - tolerance;
            this.maxAngle = initialValue + tolerance;
        }

        public override double GetAngle(Body body)
        {
            return Calculations.GetAngle(
                body.Joints[activeJoint.KinectJoint.Type], body.Joints[activeJoint.KinectJoint.Parent.Type], body.Joints[passiveJoint.KinectJoint.Type]
            );
        }

        public override bool IsFulfilled(Body body)
        {
            this.angle = base.GetAngle(body);

            isFulfilled = minAngle <= angle && angle <= maxAngle;

            return isFulfilled;
        }

        public override void Write(Body body)
        {
            var angleString = value.ToString("000");

            if (angleString[0] == '0')
                angleString = angleString.Substring(1);

            feedback.ShowInformation(string.Format(information, activeJoint.KinectJoint.Translation, angleString));
        }

        public override void Draw(Body body)
        {
            double correctedAngle = angle;

            if (angle > value)
                correctedAngle = angle - tolerance;
            else if (correctedAngle < value)
                correctedAngle = angle + tolerance;

            feedback.DrawCircle(initialValue, correctedAngle, activeJoint.KinectJoint);
        }

        public override void PlayValue()
        {
            feedback.PitchValue(base.pitchType, this.initialValue, this.angle);
        }

        public override void Clear()
        {
            feedback.ClearDrawings();
        }
    }
}
