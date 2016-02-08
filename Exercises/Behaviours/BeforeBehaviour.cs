namespace HSA.RehaGame.Exercises.Behaviours
{
    using System;
    using FulFillables;
    using DB;
    using InGame;
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;

    public class BeforeBehaviour : BaseJointBehaviour
    {
        public BeforeBehaviour(string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base(unityObjectName, activeJoint, passiveJoint, dbManager, settings, drawing, previous)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = body.Joints[activeJoint.JointType].Position.Z < body.Joints[passiveJoint.JointType].Position.Z;
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
