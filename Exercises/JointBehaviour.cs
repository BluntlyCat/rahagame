namespace HSA.RehaGame.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Kinect = Windows.Kinect;
    using System.Text;
    using User;
    using UnityEngine;
    using UnityEngine.UI;

    public class JointBehaviour
    {
        private JointBehaviours behaviour;
        private string value;

        private PatientJoint activeJoint;
        private PatientJoint passiveJoint;

        private JointBehaviour nextBehaviour;
        private bool expectedBehaviour = false;

        public JointBehaviour(JointBehaviours behaviour, PatientJoint activeJoint, PatientJoint passiveJoint)
        {
            this.behaviour = behaviour;
            this.value = null;

            this.activeJoint = activeJoint;
            this.passiveJoint = passiveJoint;
        }

        public JointBehaviour(JointBehaviours behaviour, string value, PatientJoint activeJoint, PatientJoint passiveJoint)
        {
            this.behaviour = behaviour;
            this.value = value;

            this.activeJoint = activeJoint;
            this.passiveJoint = passiveJoint;
        }

        public void AddNextBehaviour(JointBehaviour behaviour)
        {
            this.nextBehaviour = behaviour;
        }

        public bool CheckBehaviour(Kinect.Body body)
        {
            var active = body.Joints[activeJoint.JointType];
            var passive = body.Joints[passiveJoint.JointType];

            switch (this.behaviour)
            {
                case JointBehaviours.above:
                    if (active.Position.Y > passive.Position.Y)
                    {
                        expectedBehaviour = true;
                    }
                    break;

                case JointBehaviours.beneath:
                    if (active.Position.Y < passive.Position.Y)
                    {
                        expectedBehaviour = true;
                    }
                    break;
            }

            return expectedBehaviour;
        }

        public JointBehaviour NextBehaviour
        {
            get
            {
                return nextBehaviour;
            }
        }

        public bool ExpectedBehaviour
        {
            get
            {
                return expectedBehaviour;
            }
        }

        public PatientJoint ActiveJoint
        {
            get
            {
                return activeJoint;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", behaviour.ToString(), ExpectedBehaviour);
        }
    }
}
