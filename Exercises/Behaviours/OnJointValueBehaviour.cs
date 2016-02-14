namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB;
    using DB.Models;
    using FulFillables;
    using UI.VisualExercise;
    using Windows.Kinect;

    public class OnJointValueBehaviour : DistanceValueBehaviour
    {
        public OnJointValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base(value, unityObjectName, activeJoint, passiveJoint, dbManager, settings, drawing, previous)
        {

        }

        public override void Write(Body body)
        {
            drawing.ShowInformation(string.Format(information, activeJoint.Translation, passiveJoint.Translation));
        }

        public override void Draw(Body body)
        {
            drawing.DrawLine(body.Joints[activeJoint.Type], body.Joints[passiveJoint.Type], .25f);
        }
    }
}
