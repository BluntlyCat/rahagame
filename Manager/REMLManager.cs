namespace HSA.RehaGame.Manager
{
    using Audio;
    using DB = DB.Models;
    using Exercises;
    using Exercises.Actions;
    using Exercises.Behaviours;
    using Exercises.FulFillables;
    using Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using UI.Feedback;
    public class REMLManager
    {
        private static Logger<REMLManager> logger = new Logger<REMLManager>();
        private SettingsManager settingsManager;
        private WriteStatisticManager statisticManager;
        private Feedback feedback;
        private DB.Patient patient;
        private string rel;

        private Exercise exercise;

        public REMLManager(DB.Patient patient, SettingsManager settingsManager, WriteStatisticManager statisticManager, Feedback feedback, string rel)
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.patient = patient;
            this.settingsManager = settingsManager;
            this.statisticManager = statisticManager;
            this.feedback = feedback;
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

        private Exercise CreateExercise(XmlReader reader)
        {
            var attributes = GetAttributes(reader);

            string exerciseName = "";
            int exerciseRepititions = 1;

            if (attributes.ContainsKey("name"))
                exerciseName = attributes["name"];

            if (attributes.ContainsKey("repititions"))
                exerciseRepititions = int.Parse(attributes["repititions"]);

            return new Exercise(exerciseName, DB.StatisticType.exercise, null, null, exerciseRepititions, statisticManager);
        }

        private BaseAction CreateAction(XmlReader reader, FulFillable lastToDoAble)
        {
            BaseAction action = null;

            var attributes = GetAttributes(reader);
            var actionName = attributes["name"];
            int repititions = 1;

            if (attributes.ContainsKey("repititions"))
                repititions = int.Parse(attributes["repititions"]);

            if (actionName == "hold")
            {
                action = new HoldAction(actionName, DB.StatisticType.action, null, double.Parse(attributes["value"]), lastToDoAble, settingsManager, feedback, PitchType.pitchDefault, repititions, statisticManager);
            }

            else if (actionName == "wait")
            {
                action = new WaitAction(actionName, DB.StatisticType.action, null, double.Parse(attributes["value"]), lastToDoAble, settingsManager, feedback, PitchType.pitchDefault, repititions, statisticManager);
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

            var active = patient.GetJointByName(((Joint)lastToDoAble).Name);

            int repititions = 1;

            if (attributes.ContainsKey("repititions"))
                repititions = int.Parse(attributes["repititions"]);

            if (attributes.ContainsKey("joint"))
            {
                var passive = patient.GetJointByName(attributes["joint"]);

                if (attributes.ContainsKey("value"))
                {
                    var value = double.Parse(attributes["value"]);

                    if (behaviourName == "distance")
                        behaviour = new DistanceValueBehaviour(value, behaviourName, DB.StatisticType.behaviour, active, active, passive, settingsManager, feedback, PitchType.pitchDistance, lastBehaviour, repititions, statisticManager);

                    else if (behaviourName == "on")
                        behaviour = new OnJointValueBehaviour(value, behaviourName, DB.StatisticType.behaviour, active, active, passive, settingsManager, feedback, PitchType.pitchOnJoint, lastBehaviour, repititions, statisticManager);
                }
                else
                {
                    if (behaviourName == "above")
                        behaviour = new AboveBehaviour(behaviourName, DB.StatisticType.behaviour, active, active, passive, settingsManager, feedback, PitchType.pitchHeight, lastBehaviour, repititions, statisticManager);
                    else if (behaviourName == "below")
                        behaviour = new BelowBehaviour(behaviourName, DB.StatisticType.behaviour, active, active, passive, settingsManager, feedback, PitchType.pitchHeight, lastBehaviour, repititions, statisticManager);
                    else if (behaviourName == "behind")
                        behaviour = new BehindBehaviour(behaviourName, DB.StatisticType.behaviour, active, active, passive, settingsManager, feedback, PitchType.pitchDepth, lastBehaviour, repititions, statisticManager);
                    else if (behaviourName == "before")
                        behaviour = new BeforeBehaviour(behaviourName, DB.StatisticType.behaviour, active, active, passive, settingsManager, feedback, PitchType.pitchDepth, lastBehaviour, repititions, statisticManager);
                }
            }
            else if (attributes.ContainsKey("value"))
            {
                DB.PatientJoint childJoint = null;
                var value = double.Parse(attributes["value"]);

                if (attributes.ContainsKey("toChild"))
                    childJoint = patient.GetJointByName(attributes["toChild"]);
                else
                    childJoint = patient.GetJointByName(active.KinectJoint.Children.First().Value.Name);

                if (behaviourName == "angle")
                    behaviour = new AngleValueBehaviour(value, behaviourName, DB.StatisticType.behaviour, active, active, childJoint, settingsManager, feedback, PitchType.pitchAngleValue, lastBehaviour, repititions, statisticManager);
            }

            if (lastBehaviour != null)
                lastBehaviour.AddNext(behaviour);

            return behaviour;
        }

        private Joint CreateJoint(XmlReader reader, FulFillable lastToDoAble)
        {
            var attributes = GetAttributes(reader);

            int repititions = 1;

            if (attributes.ContainsKey("repititions"))
                repititions = int.Parse(attributes["repititions"]);

            Joint joint = new Joint(attributes["name"], DB.StatisticType.joint, null, settingsManager, feedback, PitchType.pitchDefault, lastToDoAble, repititions, statisticManager);

            if (lastToDoAble != null)
                lastToDoAble.AddNext(joint);

            return joint;
        }

        private Step CreateStep(XmlReader reader, BaseStep lastStep)
        {
            var attributes = GetAttributes(reader);
            var name = attributes.ContainsKey("name") ? attributes["name"] : "";

            int repititions = 1;

            if (attributes.ContainsKey("repititions"))
                repititions = int.Parse(attributes["repititions"]);

            Step step = new Step(name, DB.StatisticType.step, null, lastStep, repititions, statisticManager);

            if (lastStep != null)
                lastStep.AddNext(step);

            return step;
        }

        private void ReadExerciseAttributes(XmlReader reader)
        {
            
        }

        public Exercise ParseRGML()
        {
            IList<RelNodeTypes> nodeTypes = new List<RelNodeTypes>();

            Step lastStep = null;
            Step firstStep = null;

            BaseJointBehaviour lastBehaviour = null;
            BaseJointBehaviour firstBehaviour = null;

            Informable lastInformable = null;
            Informable firstInformable = null;

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
                                    exercise = CreateExercise(reader);
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
                                    var newJoint = CreateJoint(reader, lastInformable);

                                    if (firstInformable == null)
                                        firstInformable = newJoint;

                                    lastInformable = newJoint;
                                    break;

                                case RelNodeTypes.action:
                                    var newAction = CreateAction(reader, lastInformable);

                                    if (firstInformable == null)
                                        firstInformable = newAction;

                                    lastInformable = newAction;
                                    break;

                                case RelNodeTypes.behaviour:
                                    var newBehaviour = CreateJointBehaviour(reader, lastInformable, lastBehaviour);

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
                                    lastStep.SetFirstInformable(firstInformable as Informable);
                                    firstInformable = lastInformable = null;
                                    break;

                                case RelNodeTypes.stepGroup:
                                    break;

                                case RelNodeTypes.joint:
                                    lastInformable.Convert<Joint>().SetFirstBehaviour(firstBehaviour);
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

            exercise.SetFirstStep(firstStep);

            return exercise;
        }
    }
}
