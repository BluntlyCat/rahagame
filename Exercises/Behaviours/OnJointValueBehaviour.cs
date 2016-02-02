﻿namespace HSA.RehaGame.Exercises.Behaviours
{
    using FulFillables;
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;

    public class OnJointValueBehaviour : DistanceValueBehaviour
    {
        public OnJointValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Drawing drawing, FulFillable previous) : base(value, unityObjectName, activeJoint, passiveJoint, drawing, previous)
        {

        }

        public override string Information()
        {
            return string.Format(information, activeJoint.Translation, passiveJoint.Translation);
        }

        public override void Draw(Body body)
        {
            drawing.DrawLine(body.Joints[activeJoint.JointType], body.Joints[passiveJoint.JointType], .25f);
        }
    }
}