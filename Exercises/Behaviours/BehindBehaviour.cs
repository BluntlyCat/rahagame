namespace HSA.RehaGame.Exercises.Behaviours
{
    using System;
    using FulFillables;
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;

    public class BehindBehaviour : BaseJointBehaviour
    {
        public BehindBehaviour(string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Drawing drawing, FulFillable previous) : base(unityObjectName, activeJoint, passiveJoint, drawing, previous)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = body.Joints[activeJoint.JointType].Position.Z > body.Joints[passiveJoint.JointType].Position.Z;
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
