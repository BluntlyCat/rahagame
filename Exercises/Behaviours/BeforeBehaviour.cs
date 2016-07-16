namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB.Models;
    using FulFillables;
    using Logging;
    using Manager;
    using Manager.Audio;
    using UI.Feedback;
    using Windows.Kinect;
    public class BeforeBehaviour : BaseJointBehaviour
    {
        private double handLeftZ;
        private double handRightZ;

        private double activeJointZ;
        private double passiveJointZ;

        public BeforeBehaviour(string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, PatientJoint activeJoint, PatientJoint passiveJoint, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base(unityObjectName, statisticType, affectedJoint, activeJoint, passiveJoint, settingsManager, feedback, pitchType, previous, repetitions, statisticManager)
        {
        }

        public override bool IsFulfilled(Body body)
        {
            handLeftZ = body.Joints[JointType.HandLeft].Position.Z;
            handRightZ = body.Joints[JointType.HandRight].Position.Z;

            activeJointZ = body.Joints[activeJoint.KinectJoint.Type].Position.Z;
            passiveJointZ = body.Joints[passiveJoint.KinectJoint.Type].Position.Z;

            isFulfilled = activeJointZ < passiveJointZ;
            return isFulfilled;
        }

        public override void Draw(Body body)
        {
            base.Draw(body);
        }

        public override void PlayValue()
        {
            feedback.PitchValue(base.pitchType, handLeftZ - handRightZ, activeJointZ, passiveJointZ);
        }

        public override void Clear()
        {
            feedback.ClearDrawings();
        }
    }
}
