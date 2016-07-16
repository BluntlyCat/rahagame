namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB.Models;
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using UI.Feedback;
    using Windows.Kinect;

    public class OnJointValueBehaviour : DistanceValueBehaviour
    {
        public OnJointValueBehaviour(double value, string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, PatientJoint activeJoint, PatientJoint passiveJoint, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base(value, unityObjectName, statisticType, affectedJoint, activeJoint, passiveJoint, settingsManager, feedback, pitchType, previous, repetitions, statisticManager)
        {

        }

        public override void Write(Body body)
        {
            feedback.ShowInformation(string.Format(information, activeJoint.KinectJoint.Translation, passiveJoint.KinectJoint.Translation));
        }

        public override void Draw(Body body)
        {
            base.Draw(body);
            feedback.DrawLine(body.Joints[activeJoint.KinectJoint.Type], body.Joints[passiveJoint.KinectJoint.Type], .25f);
        }
    }
}
