namespace HSA.RehaGame.Exercises.FulFillables
{
    using System;
    using System.Collections.Generic;
    using Logging;
    using Windows.Kinect;

    public class Step : BaseStep
    {
        private static Logger<Step> logger = new Logger<Step>();

        private IList<Joint> joints = new List<Joint>();
        private Joint currentJoint;

        public Step(string description, BaseStep prevStep) : base(description, prevStep)
        {
            if (current == null)
                current = this;

            logger.AddLogAppender<ConsoleAppender>();
        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = CheckJoints(body);

            if (isFulfilled)
                isFulfilled = CheckActions(body);

            return isFulfilled;
        }

        public override string Information()
        {
            if (currentAction != null)
                information = currentAction.Information();

            else if (currentJoint != null)
                information = currentJoint.Information();

            return information;
        }

        public override void VisualInformation(Body body)
        {
            if (currentAction != null)
                currentAction.VisualInformation(body);

            else if (currentJoint != null)
                currentJoint.VisualInformation(body);
        }

        public override void Debug(Body body)
        {
            if (currentAction != null)
                currentAction.Debug(body);

            else if (currentJoint != null)
                currentJoint.Debug(body);
        }

        public void AddJoint(Joint joint)
        {
            this.joints.Add(joint);
        }

        public Joint CurrentJoint
        {
            get
            {
                return currentJoint;
            }
        }

        private bool CheckJoints(Body body)
        {
            foreach (var joint in joints)
            {
                currentJoint = joint;

                if (joint.IsFulfilled(body) == false)
                {
                    ResetActions();
                    return false;
                }
            }

            return true;
        }
    }
}
