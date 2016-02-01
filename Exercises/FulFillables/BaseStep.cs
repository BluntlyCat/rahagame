namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using Actions;
    using UI.VisualExercise;
    using Windows.Kinect;

    public abstract class BaseStep : Informable
    {
        protected IList<BaseAction> actions = new List<BaseAction>();
        protected BaseAction currentAction;
        protected BaseStep current;
        protected BaseStep previous;
        protected BaseStep next;

        private string description;

        public BaseStep(string description, BaseStep previous, Drawing drawing) : base (drawing)
        {
            this.description = description;
            this.previous = previous;
        }

        public void AddAction(BaseAction action)
        {
            this.actions.Add(action);
        }

        public void AddNext(BaseStep next)
        {
            this.next = next;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}: {2}", this.GetType().Name, description, isFulfilled);
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public BaseStep First
        {
            get
            {
                var current = this;

                while (current.previous != null)
                    current = current.previous;

                return current;
            }
        }

        public BaseStep Last
        {
            get
            {
                var current = this;

                while (current.next != null)
                    current = current.next;

                return current;
            }
        }

        public BaseStep Current
        {
            get
            {
                return current;
            }
        }

        public BaseStep Previous
        {
            get
            {
                return previous;
            }
        }

        public BaseStep Next
        {
            get
            {
                return next;
            }
        }

        protected bool CheckActions(Body body)
        {
            foreach (var action in actions)
            {
                currentAction = action;

                if (action.IsFulfilled(body) == false)
                    return false;
            }

            return true;
        }

        protected void ResetActions()
        {
            foreach (var action in actions)
            {
                action.Reset();
            }

            currentAction = null;
        }
    }
}
