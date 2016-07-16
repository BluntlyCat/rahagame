namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB.Models;
    using FulFillables;
    using Logging;
    using Manager;
    using Manager.Audio;
    using UI.Feedback;
    using Windows.Kinect;
    public class AboveBehaviour : BaseJointBehaviour
    {
        private double headY;
        private double footY;

        private double activeJointY;
        private double passiveJointY;

        public AboveBehaviour(string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, PatientJoint activeJoint, PatientJoint passiveJoint, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base(unityObjectName, statisticType, affectedJoint, activeJoint, passiveJoint, settingsManager, feedback, pitchType, previous, repetitions, statisticManager)
        {
        }

        public override bool IsFulfilled(Body body)
        {
            headY = body.Joints[JointType.Head].Position.Y;
            footY = body.Joints[JointType.FootLeft].Position.Y;

            activeJointY = body.Joints[activeJoint.KinectJoint.Type].Position.Y;
            passiveJointY = body.Joints[passiveJoint.KinectJoint.Type].Position.Y;

            isFulfilled = activeJointY > passiveJointY;
            return isFulfilled;
        }

        public override void Draw(Body body)
        {
            base.Draw(body);
        }

        public override void PlayValue()
        {
            feedback.PitchValue(base.pitchType, headY - footY, activeJointY, passiveJointY);
        }

        public override void Clear()
        {
            feedback.ClearDrawings();
        }
    }
}
