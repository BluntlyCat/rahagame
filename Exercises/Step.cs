namespace HSA.RehaGame.Exercises
{
    using System;
    using System.Collections.Generic;
    using Kinect = Windows.Kinect;
    using System.Linq;
    using System.Text;

    public class Step
    {
        private IDictionary<string, string> attributes = new Dictionary<string, string>();
        private JointBehaviour jointBehaviour;

        private Step nextStep;
        private bool stepDone = false;

        public Step(IDictionary<string, string> attributes)
        {
            this.attributes = attributes;
        }

        public string GetAttribute(string key)
        {
            if(attributes.ContainsKey(key))
                return attributes[key];

            return "";
        }

        public void AddNextStep(Step step)
        {
            this.nextStep = step;
        }

        public void SetFirstBehaviour(JointBehaviour behaviour)
        {
            this.jointBehaviour = behaviour;
        }

        public bool DoStep(Kinect.Body body)
        {
            bool expectedBehaviour = false;

            if (jointBehaviour.ExpectedBehaviour)
                jointBehaviour = jointBehaviour.NextBehaviour;

            else if (jointBehaviour.ExpectedBehaviour && jointBehaviour.NextBehaviour == null)
                this.stepDone = true;

            else
                expectedBehaviour = jointBehaviour.CheckBehaviour(body);

            return stepDone && expectedBehaviour;
        }

        public Step NextStep
        {
            get
            {
                return nextStep;
            }
        }

        public bool StepDone
        {
            get
            {
                return stepDone;
            }
        }

        public JointBehaviour Behaviour
        {
            get
            {
                return jointBehaviour;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", GetAttribute("name"), StepDone);
        }
    }
}
