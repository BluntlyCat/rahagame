namespace HSA.RehaGame.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using User;

    public class ExecutionLanguage
    {
        private string rel;

        public ExecutionLanguage(string rel)
        {
            this.rel = rel;
        }

        private IDictionary<string, string> GetAttributes(XmlReader reader)
        {
            IDictionary<string, string> attributes = new Dictionary<string, string>();

            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                attributes.Add(reader.Name, reader.Value);
            }

            return attributes;
        }

        private bool IsBehaviour(string key)
        {
            bool isBehaviour = false;

            foreach(JointBehaviours behaviour in Enum.GetValues(typeof(JointBehaviours)))
            {
                if(behaviour.ToString() == key)
                {
                    isBehaviour = true;
                    break;
                }
            }

            return isBehaviour;
        }

        private JointBehaviour CreateBehaviour(string action, string value, PatientJoint active, PatientJoint passive, JointBehaviour lastBehaviour)
        {
            if(IsBehaviour(action))
            {
                JointBehaviours behaviour = (JointBehaviours)Enum.Parse(typeof(JointBehaviours), action);
                JointBehaviour newBehaviour = new JointBehaviour(behaviour, value, active, passive);

                if (lastBehaviour != null)
                    lastBehaviour.AddNextBehaviour(newBehaviour);

                return newBehaviour;
            }

            throw new Exception();
        }

        private bool HasActionValue(string action)
        {
            return action.Contains(':');
        }

        private JointBehaviour ParseBehaviours(string stringActions, PatientJoint active, PatientJoint passive)
        {
            JointBehaviour firstBehaviour = null;
            JointBehaviour lastBehaviour = null;

            string[] actions = stringActions.Split(';');

            foreach(var actionKeyValue in actions)
            {
                string action = null;
                string value = null;

                if(HasActionValue(actionKeyValue))
                {
                    string[] keyValueParts = actionKeyValue.Split(':');
                    action = keyValueParts[0];
                    value = keyValueParts[1];
                }
                else
                {
                    action = actionKeyValue;
                }

                lastBehaviour = CreateBehaviour(action, value, active, passive, lastBehaviour);

                if (firstBehaviour == null)
                    firstBehaviour = lastBehaviour;
            }

            return firstBehaviour;
        }

        private JointBehaviour CreateBehaviours(XmlReader reader, Exercise exercise)
        {
            JointBehaviour firstBehaviour = null;
            
            PatientJoint active = null;
            PatientJoint passive = null;

            var attributes = GetAttributes(reader);
            
            foreach(var attribute in attributes)
            {
                if (attribute.Key == "active")
                    active = exercise.Patient.GetJoint(attribute.Value);

                else if (attribute.Key == "passive")
                    passive = exercise.Patient.GetJoint(attribute.Value);

                else if (attribute.Key == "behaviours")
                {
                    firstBehaviour = ParseBehaviours(attribute.Value, active, passive);
                }
            }

            if(firstBehaviour != null && active != null && passive != null)
                return firstBehaviour;

            throw new Exception();
        }

        private Step CreateStep(XmlReader reader, Step lastStep)
        {
            Step step = new Step(GetAttributes(reader));

            if (lastStep != null)
                lastStep.AddNextStep(step);

            return step;
        }

        public Step GetSteps(Exercise exercise)
        {
            IList<Step> steps = new List<Step>();
            Step lastStep = null;
            Step firstStep = null;

            using (XmlReader reader = XmlReader.Create(new StringReader(rel)))
            {
                while (reader.Read())
                {
                    if (reader.Name != "")
                    {
                        RelNodeTypes nodeType = (RelNodeTypes)Enum.Parse(typeof(RelNodeTypes), reader.Name);

                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                switch (nodeType)
                                {
                                    case RelNodeTypes.step:
                                        lastStep = CreateStep(reader, lastStep);

                                        if (firstStep == null)
                                            firstStep = lastStep;

                                        steps.Add(lastStep);
                                        break;

                                    case RelNodeTypes.joint:
                                        if(lastStep != null)
                                            lastStep.SetFirstBehaviour(CreateBehaviours(reader, exercise));
                                        break;
                                }
                                break;
                        }
                    }
                }
            }

            return firstStep;
        }
    }
}
