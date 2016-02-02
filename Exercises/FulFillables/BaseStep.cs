namespace HSA.RehaGame.Exercises.FulFillables
{
    using DB;
    using Actions;
    using Windows.Kinect;

    public abstract class BaseStep : Informable
    {
        protected Informable firstToDoAble;
        protected Informable currentToDoAble;

        protected string unityObjectName;

        public BaseStep(string unityObjectName, FulFillable previous) : base (previous)
        {
            this.unityObjectName = unityObjectName;

            if (unityObjectName != null)
                this.information = DBManager.GetStepInformation(unityObjectName).GetValueFromLanguage("order");
        }

        public override bool IsFulfilled(Body body)
        {
            // ToDo Was ist wenn man die Position vom vorherigen Schritt verlässt?

            if (currentToDoAble.GetType().IsSubclassOf(typeof(BaseAction)) && currentToDoAble.Previous != null)
            {
                isFulfilled = currentToDoAble.Previous.IsFulfilled(body);

                if (isFulfilled)
                    isFulfilled = currentToDoAble.IsFulfilled(body);
                else
                    ((BaseAction)currentToDoAble).Reset();
            }
            else
                isFulfilled = currentToDoAble.IsFulfilled(body);

            if (isFulfilled)
            {
                if (currentToDoAble.Next == null)
                {
                    isFulfilled = true;
                }
                else
                {
                    currentToDoAble = currentToDoAble.Next as Informable;
                    isFulfilled = false;
                }
            }

            return isFulfilled;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}: {2}", this.GetType().Name, unityObjectName, isFulfilled);
        }

        public string Description
        {
            get
            {
                return unityObjectName;
            }
        }

        protected void ResetAction()
        {
            if(currentToDoAble.GetType() == typeof(BaseAction))
                ((BaseAction)currentToDoAble).Reset();
        }
    }
}
