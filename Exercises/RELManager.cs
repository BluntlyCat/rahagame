namespace HSA.RehaGame.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using Actions;
    using Behaviours;
    using FulFillables;
    using User;
    using Logging;
    using UI.VisualExercise;

    public class RELManager
    {
        private static Logger<RELManager> logger = new Logger<RELManager>();
        private Drawing drawing;
        private Patient patient;
        private string rel;

        public RELManager(Patient patient, Drawing drawing, string rel)
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.patient = patient;
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

        private BaseAction CreateAction(XmlReader reader, FulFillable lastToDoAble)
        {
            BaseAction action = null;

            var attributes = GetAttributes(reader);
            var actionName = attributes["name"];

            if (actionName == "hold")
            {
                action = new HoldAction(actionName, double.Parse(attributes["value"]), lastToDoAble);
            }

            else if (actionName == "repeat")
            {
                action = new RepeatAction(actionName, double.Parse(attributes["value"]), lastToDoAble);
            }

            else if (actionName == "wait")
            {
                action = new WaitAction(actionName, double.Parse(attributes["value"]), lastToDoAble);
            }

            if (lastToDoAble != null)
                lastToDoAble.AddNext(action);

            return action;
        }

        private BaseJointBehaviour CreateJointBehaviour(XmlReader reader, FulFillable lastToDoAble, BaseJointBehaviour lastBehaviour)
        {
            BaseJointBehaviour behaviour = null;

            var attributes = GetAttributes(reader);
            var behaviourName = attributes["is"];

            var active = patient.GetJoint(((Joint)lastToDoAble).Name);

            if (attributes.ContainsKey("joint"))
            {
                var passive = patient.GetJoint(attributes["joint"]);

                if (attributes.ContainsKey("value"))
                {
                    var value = double.Parse(attributes["value"]);

                    if (behaviourName == "distance")
                        behaviour = new DistanceValueBehaviour(value, behaviourName, active, passive, drawing, lastToDoAble);

                    else if (behaviourName == "on")
                        behaviour = new OnJointValueBehaviour(value, behaviourName, active, passive, drawing, lastToDoAble);
                }
                else
                {
                    if (behaviourName == "above")
                        behaviour = new AboveBehaviour(behaviourName, active, passive, drawing, lastToDoAble);
                    else if (behaviourName == "below")
                        behaviour = new BelowBehaviour(behaviourName, active, passive, drawing, lastToDoAble);
                    else if (behaviourName == "behind")
                        behaviour = new BehindBehaviour(behaviourName, active, passive, drawing, lastToDoAble);
                    else if (behaviourName == "before")
                        behaviour = new BeforeBehaviour(behaviourName, active, passive, drawing, lastToDoAble);
                }
            }
            else if (attributes.ContainsKey("value"))
            {
                var value = double.Parse(attributes["value"]);

                if (behaviourName == "angle")
                    behaviour = new AngleValueBehaviour(value, behaviourName, active, null, drawing, lastToDoAble);
            }

            if (lastBehaviour != null)
                lastBehaviour.AddNext(behaviour);

            return behaviour;
        }

        private Joint CreateJoint(XmlReader reader, FulFillable lastToDoAble)
        {
            var attributes = GetAttributes(reader);

            Joint joint = new Joint(attributes["name"], lastToDoAble);

            if (lastToDoAble != null)
                lastToDoAble.AddNext(joint);

            return joint;
        }

        private Step CreateStep(XmlReader reader, BaseStep lastStep)
        {
            var attributes = GetAttributes(reader);
            var name = attributes.ContainsKey("unityObjectName") ? attributes["unityObjectName"] : null;

            Step step = new Step(name, lastStep);

            if (lastStep != null)
                lastStep.AddNext(step);

            return step;
        }

        public BaseStep GetSteps()
        {
            IList<RelNodeTypes> nodeTypes = new List<RelNodeTypes>();

            Step lastStep = null;
            Step firstStep = null;

            BaseJointBehaviour lastBehaviour = null;
            BaseJointBehaviour firstBehaviour = null;

            Informable lastToDoAble = null;
            Informable firstToDoAble = null;

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
                                    var newJoint = CreateJoint(reader, lastToDoAble);

                                    if (firstToDoAble == null)
                                        firstToDoAble = newJoint;

                                    lastToDoAble = newJoint;
                                    break;

                                case RelNodeTypes.action:
                                    var newAction = CreateAction(reader, lastToDoAble);

                                    if (firstToDoAble == null)
                                        firstToDoAble = newAction;

                                    lastToDoAble = newAction;
                                    break;

                                case RelNodeTypes.behaviour:
                                    var newBehaviour = CreateJointBehaviour(reader, lastToDoAble, lastBehaviour);

                                    if (firstBehaviour == null)
                                        firstBehaviour = newBehaviour;

                                    lastBehaviour = newBehaviour;
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
                            switch (lastNodeType)
                            {
                                case RelNodeTypes.behaviour:
                                    break;

                                case RelNodeTypes.action:
                                    break;

                                case RelNodeTypes.exercise:
                                    break;

                                case RelNodeTypes.step:
                                    lastStep.SetFirstToDoAble(firstToDoAble);
                                    firstToDoAble = lastToDoAble = null;
                                    break;

                                case RelNodeTypes.stepGroup:
                                    break;

                                case RelNodeTypes.joint:
                                    ((Joint)lastToDoAble).SetFirstBehaviour(firstBehaviour);
                                    firstBehaviour = lastBehaviour = null;
                                    break;
                            }

                            nodeTypes.RemoveAt(nodeTypes.Count - 1);

                            if (nodeTypes.Count > 0)
                                lastNodeType = nodeTypes[nodeTypes.Count - 1];
                            break;
                    }
                }
            }

            return firstStep;
        }
    }
}
