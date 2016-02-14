namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB;
    using DB.Models;
    using FulFillables;
    using UI.VisualExercise;
    using Windows.Kinect;

    public class BeforeBehaviour : BaseJointBehaviour
    {
        public BeforeBehaviour(string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base(unityObjectName, activeJoint, passiveJoint, dbManager, settings, drawing, previous)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = body.Joints[activeJoint.Type].Position.Z < body.Joints[passiveJoint.Type].Position.Z;
            return isFulfilled;
        }

        public override void Draw(Body body)
        {

        }

        public override void Clear()
        {
            drawing.ClearDrawings();
        }
    }
}
