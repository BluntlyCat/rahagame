namespace HSA.RehaGame.Exercises.Behaviours
{
    using System;
    using User;
    using Windows.Kinect;

    public class BelowBehaviour : BaseJointBehaviour
    {
        public BelowBehaviour(string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint) : base(unityObjectName, activeJoint, passiveJoint)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = body.Joints[activeJoint.JointType].Position.Y < body.Joints[passiveJoint.JointType].Position.Y;
            return isFulfilled;
        }

        public override void VisualInformation(Body body)
        {
            
        }
    }
}
