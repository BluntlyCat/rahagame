namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB.Models;
    using FulFillables;
    using Logging;
    using Manager;
    using Manager.Audio;
    using Math;
    using UI.Feedback;
    using UnityEngine;
    using Windows.Kinect;
    public class DistanceValueBehaviour : BaseJointValueBehaviour
    {
        private Vector3 maxDistance;
        private Vector3 distance;

        public DistanceValueBehaviour(double value, string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, PatientJoint activeJoint, PatientJoint passiveJoint, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base(value, unityObjectName, statisticType, affectedJoint, activeJoint, passiveJoint, settingsManager, feedback, pitchType, previous, repetitions, statisticManager)
        {
            logger.AddLogAppender<ConsoleAppender>();
        }

        public override bool IsFulfilled(Body body)
        {
            var active = body.Joints[activeJoint.KinectJoint.Type];
            var passive = body.Joints[passiveJoint.KinectJoint.Type];

            maxDistance = Calculations.GetDistance(body.Joints[JointType.Head], body.Joints[JointType.FootLeft]);
            distance = Calculations.GetDistance(active, passive);

            isFulfilled = distance.x <= value && distance.y <= value && distance.z <= value;

            return isFulfilled;
        }

        public override void Write(Body body)
        {
            feedback.ShowInformation(string.Format(information, activeJoint.KinectJoint.Translation, passiveJoint.KinectJoint.Translation, value));
        }

        public override void Draw(Body body)
        {
            base.Draw(body);
            feedback.DrawLine(body.Joints[activeJoint.KinectJoint.Type], body.Joints[passiveJoint.KinectJoint.Type], 1f);
        }

        public override void PlayValue()
        {
            feedback.PitchValue(base.pitchType, maxDistance, distance);
        }

        public override void Clear()
        {
            feedback.ClearDrawings();
        }
    }
}
