﻿namespace HSA.RehaGame.Exercises.Behaviours
{
    using System;
    using UI.VisualExercise;
    using Logging;
    using User;
    using Windows.Kinect;

    public class AboveBehaviour : BaseJointBehaviour
    {
        private static Logger<AboveBehaviour> logger = new Logger<AboveBehaviour>();

        public AboveBehaviour(string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Drawing drawing) : base(unityObjectName, activeJoint, passiveJoint, drawing)
        {
            logger.AddLogAppender<ConsoleAppender>();
        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = body.Joints[activeJoint.JointType].Position.Y > body.Joints[passiveJoint.JointType].Position.Y;
            return isFulfilled;
        }

        public override void VisualInformation(Body body)
        {
            
        }
    }
}