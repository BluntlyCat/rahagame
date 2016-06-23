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
        public OnJointValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous) : base(value, unityObjectName, activeJoint, passiveJoint, settingsManager, feedback, pitchType, previous)
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
