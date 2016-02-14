namespace HSA.RehaGame.Manager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using DB;
    using Exercises;
    using Exercises.Actions;
    using Exercises.Behaviours;
    using Exercises.FulFillables;
    using Logging;
    using Models = DB.Models;
    using UI = UI.VisualExercise;

    public class REMLManager
    {
        private static Logger<REMLManager> logger = new Logger<REMLManager>();
        private Database dbManager;
        private Models.Settings settings;
        private UI.Drawing drawing;
        private Models.Patient patient;
        private string rel;

        public REMLManager(Models.Patient patient, Database dbManager, Models.Settings settings, UI.Drawing drawing, string rel)
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.patient = patient;
            this.dbManager = dbManager;
            this.settings = settings;
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
                action = new HoldAction(actionName, double.Parse(attributes["value"]), lastToDoAble, dbManager, settings, drawing);
            }

            else if (actionName == "repeat")
            {
                action = new RepeatAction(actionName, double.Parse(attributes["value"]), lastToDoAble, dbManager, settings, drawing);
            }

            else if (actionName == "wait")
            {
                action = new WaitAction(actionName, double.Parse(attributes["value"]), lastToDoAble, dbManager, settings, drawing);
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

            var active = patient.Joints[((Joint)lastToDoAble).Name];

            if (attributes.ContainsKey("joint"))
            {
                var passive = patient.Joints[attributes["joint"]];

                if (attributes.ContainsKey("value"))
                {
                    var value = double.Parse(attributes["value"]);

                    if (behaviourName == "distance")
                        behaviour = new DistanceValueBehaviour(value, behaviourName, active, passive, dbManager, settings, drawing, lastToDoAble);

                    else if (behaviourName == "on")
                        behaviour = new OnJointValueBehaviour(value, behaviourName, active, passive, dbManager, settings, drawing, lastToDoAble);
                }
                else
                {
                    if (behaviourName == "above")
                        behaviour = new AboveBehaviour(behaviourName, active, passive, dbManager, settings, drawing, lastToDoAble);
                    else if (behaviourName == "below")
                        behaviour = new BelowBehaviour(behaviourName, active, passive, dbManager, settings, drawing, lastToDoAble);
                    else if (behaviourName == "behind")
                        behaviour = new BehindBehaviour(behaviourName, active, passive, dbManager, settings, drawing, lastToDoAble);
                    else if (behaviourName == "before")
                        behaviour = new BeforeBehaviour(behaviourName, active, passive, dbManager, settings, drawing, lastToDoAble);
                }
            }
            else if (attributes.ContainsKey("value"))
            {
                var value = double.Parse(attributes["value"]);

                if (behaviourName == "angle")
                    behaviour = new AngleValueBehaviour(value, behaviourName, active, null, dbManager, settings, drawing, lastToDoAble);
            }

            if (lastBehaviour != null)
                lastBehaviour.AddNext(behaviour);

            return behaviour;
        }

        private Joint CreateJoint(XmlReader reader, FulFillable lastToDoAble)
        {
            var attributes = GetAttributes(reader);

            Joint joint = new Joint(attributes["name"], dbManager, settings, drawing, lastToDoAble);

            if (lastToDoAble != null)
                lastToDoAble.AddNext(joint);

            return joint;
        }

        private Step CreateStep(XmlReader reader, BaseStep lastStep)
        {
            var attributes = GetAttributes(reader);
            var name = attributes.ContainsKey("unityObjectName") ? attributes["unityObjectName"] : null;

            Step step = new Step(name, lastStep, dbManager);

            if (lastStep != null)
                lastStep.AddNext(step);

            return step;
        }

        public BaseStep ParseRGML()
        {
            IList<RelNodeTypes> nodeTypes = new List<RelNodeTypes>();

            Step lastStep = null;
            Step firstStep = null;

            BaseJointBehaviour lastBehaviour = null;
            BaseJointBehaviour firstBehaviour = null;

            Drawable lastDrawable = null;
            Drawable firstDrawable = null;

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
                                    var newJoint = CreateJoint(reader, lastDrawable);

                                    if (firstDrawable == null)
                                        firstDrawable = newJoint;

                                    lastDrawable = newJoint;
                                    break;

                                case RelNodeTypes.action:
                                    var newAction = CreateAction(reader, lastDrawable);

                                    if (firstDrawable == null)
                                        firstDrawable = newAction;

                                    lastDrawable = newAction;
                                    break;

                                case RelNodeTypes.behaviour:
                                    var newBehaviour = CreateJointBehaviour(reader, lastDrawable, lastBehaviour);

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
                                    lastStep.SetFirstDrawable(firstDrawable as Drawable);
                                    firstDrawable = lastDrawable = null;
                                    break;

                                case RelNodeTypes.stepGroup:
                                    break;

                                case RelNodeTypes.joint:
                                    ((Joint)lastDrawable).SetFirstBehaviour(firstBehaviour);
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
