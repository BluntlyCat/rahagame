namespace HSA.RehaGame.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Actions;
    using Behaviours;
    using FulFillables;
    using UI.VisualExercise;
    using Logging;

    public class ExecutionLanguage
    {
        private static Logger<ExecutionLanguage> logger = new Logger<ExecutionLanguage>();
        private Drawing drawing;
        private string rel;

        public ExecutionLanguage(Drawing drawing, string rel)
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.drawing = drawing;
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

        private BaseAction CreateAction(XmlReader reader, Exercise exercise, Joint lastJoint)
        {
            BaseAction action = null;

            var attributes = GetAttributes(reader);
            var actionName = attributes["name"];
            var baseJoint = exercise.Patient.GetJoint(lastJoint.Name);

            if (actionName == "hold")
            {
                action = new HoldAction(actionName, double.Parse(attributes["value"]), drawing);
            }

            else if (actionName == "repeat")
            {
                action = new RepeatAction(actionName, double.Parse(attributes["value"]), drawing);
            }

            else if (actionName == "open")
            {
                action = new OpenJointAction(actionName, baseJoint, double.Parse(attributes["value"]), drawing);
            }

            else if (actionName == "close")
            {
                action = new CloseJointAction(actionName, baseJoint, double.Parse(attributes["value"]), drawing);
            }

            return action;
        }

        private BaseJointBehaviour CreateJointBehaviour(XmlReader reader, Exercise exercise, Joint lastJoint)
        {
            BaseJointBehaviour behaviour = null;

            var attributes = GetAttributes(reader);
            var behaviourName = attributes["is"];

            var active = exercise.Patient.GetJoint(lastJoint.Name);
            var passive = exercise.Patient.GetJoint(attributes["joint"]);

            if (behaviourName == "above")
                behaviour = new AboveBehaviour(behaviourName, active, passive, drawing);
            else if (behaviourName == "below")
                behaviour = new BelowBehaviour(behaviourName, active, passive, drawing);

            return behaviour;
        }

        private Joint CreateJoint(XmlReader reader, Exercise exercise)
        {
            var attributes = GetAttributes(reader);

            return new Joint(attributes["name"], drawing);
        }

        private Step CreateStep(XmlReader reader, BaseStep lastStep)
        {
            var attributes = GetAttributes(reader);

            Step step = new Step(attributes["description"], lastStep, drawing);

            if (lastStep != null)
                lastStep.AddNext(step);

            return step;
        }

        public BaseStep GetSteps(Exercise exercise)
        {
            IList<RelNodeTypes> nodeTypes = new List<RelNodeTypes>();

            BaseStep lastStep = null;
            BaseStep firstStep = null;
            Joint lastJoint = null;

            using (XmlReader reader = XmlReader.Create(new StringReader(rel)))
            {
                RelNodeTypes nodeType = RelNodeTypes.none;
                RelNodeTypes lastNodeType = RelNodeTypes.none;

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            nodeType = (RelNodeTypes)Enum.Parse(typeof(RelNodeTypes), reader.Name);

                            switch (nodeType)
                            {
                                case RelNodeTypes.exercise:
                                    break;

                                case RelNodeTypes.stepGroup:
                                    break;

                                case RelNodeTypes.step:
                                    var newStep = CreateStep(reader, lastStep);

                                    if (firstStep == null)
                                        firstStep = newStep;

                                    lastStep = newStep;
                                    break;

                                case RelNodeTypes.joint:
                                    lastJoint = CreateJoint(reader, exercise);

                                    if (lastStep != null && lastStep.GetType() == typeof(Step))
                                        ((Step)lastStep).AddJoint(lastJoint);
                                    break;

                                case RelNodeTypes.action:
                                    if (lastNodeType == RelNodeTypes.step)
                                    {
                                        if (lastStep != null && lastStep.GetType() == typeof(Step))
                                            ((Step)lastStep).AddAction(CreateAction(reader, exercise, lastJoint));
                                    }
                                    else if (lastNodeType == RelNodeTypes.joint)
                                    {
                                        if (lastJoint != null)
                                            lastJoint.AddAction(CreateAction(reader, exercise, lastJoint));
                                    }
                                    break;

                                case RelNodeTypes.behaviour:
                                    if (lastNodeType == RelNodeTypes.joint && lastJoint != null)
                                        lastJoint.AddJointBehaviour(CreateJointBehaviour(reader, exercise, lastJoint));
                                    break;
                            }

                            if (nodeType != lastNodeType)
                            {
                                lastNodeType = nodeType;
                                nodeTypes.Add(nodeType);
                            }

                            break;

                        //case XmlNodeType.Whitespace:
                        case XmlNodeType.EndElement:
                            switch (nodeType)
                            {
                                case RelNodeTypes.behaviour:
                                case RelNodeTypes.action:
                                case RelNodeTypes.exercise:
                                case RelNodeTypes.step:
                                case RelNodeTypes.stepGroup:
                                case RelNodeTypes.joint:
                                    nodeTypes.RemoveAt(nodeTypes.Count - 1);

                                    if (nodeTypes.Count > 0)
                                        lastNodeType = nodeTypes[nodeTypes.Count - 1];

                                    break;
                            }
                            break;
                    }
                }
            }

            return firstStep;
        }
    }
}
