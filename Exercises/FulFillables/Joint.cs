namespace HSA.RehaGame.Exercises.FulFillables
{
    using System;
    using System.Collections.Generic;
    using Actions;
    using Behaviours;
    using User;
    using Windows.Kinect;

    public class Joint : Informable
    {
        private IList<BaseJointBehaviour> jointBehaviours = new List<BaseJointBehaviour>();
        private BaseJointBehaviour currentJointBehaviour;

        protected IList<BaseAction> actions = new List<BaseAction>();
        protected BaseAction currentAction;

        private string name;

        public Joint(string name)
        {
            this.name = name;
        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = CheckBehaviours(body);

            if (isFulfilled)
                isFulfilled = CheckActions(body);

            return isFulfilled;
        }

        public override string Information()
        {
            if(currentJointBehaviour != null)
                return currentJointBehaviour.Information();

            else if (currentAction != null)
                return currentAction.Information();

            return "";
        }

        public override void VisualInformation(Body body)
        {
            if (currentJointBehaviour != null)
                currentJointBehaviour.VisualInformation(body);

            else if (currentAction != null)
                currentAction.VisualInformation(body);
        }

        public override void Debug(Body body)
        {
            if (currentAction != null)
                currentAction.Debug(body);

            else if (currentJointBehaviour != null)
                currentJointBehaviour.Debug(body);
        }

        public void AddJointBehaviour(BaseJointBehaviour jointBehaviour)
        {
            this.jointBehaviours.Add(jointBehaviour);
        }

        public void AddAction(BaseAction action)
        {
            this.actions.Add(action);
        }

        private bool CheckBehaviours(Body body)
        {
            foreach (var behaviour in jointBehaviours)
            {
                currentJointBehaviour = behaviour;

                if (behaviour.IsFulfilled(body) == false)
                    return false;
            }

            return true;
        }

        private bool CheckActions(Body body)
        {
            foreach (var action in actions)
            {
                currentAction = action;

                if (action.IsFulfilled(body) == false)
                    return false;
            }

            return true;
        }

        private void ResetActions()
        {
            foreach (var action in actions)
            {
                action.Reset();
            }

            currentAction = null;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public BaseJointBehaviour CurrentJointBehaviour
        {
            get
            {
                return currentJointBehaviour;
            }
        }
        public BaseAction CurrentAction
        {
            get
            {
                return currentAction;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", name, isFulfilled);
        }
    }
}
