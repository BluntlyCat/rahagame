namespace HSA.RehaGame.Exercises.FulFillables
{
    using Actions;
    using DB;
    using Windows.Kinect;

    public abstract class BaseStep : FulFillable
    {
        protected Drawable firstDrawable;
        protected Drawable currentDrawable;

        protected bool prevIsAction;
        protected bool prevIsNotNull;

        protected string unityObjectName;

        public BaseStep(string unityObjectName, FulFillable previous) : base (previous)
        {
            this.unityObjectName = unityObjectName;

            if (unityObjectName != null)
                DBManager.GetExerciseInformation(unityObjectName, "step").GetValueFromLanguage("order");
        }

        private bool CheckCurrent(Body body)
        {
            isFulfilled = currentDrawable.IsFulfilled(body);

            if (isFulfilled)
            {
                if (currentDrawable.Next == null)
                {
                    isFulfilled = true;
                }
                else
                {
                    currentDrawable = currentDrawable.Next as Drawable;
                    isFulfilled = false;
                }
            }
            else
            {
                currentDrawable.Write(body);
                currentDrawable.Draw(body);
            }

            return isFulfilled;
        }

        private bool CheckPrevious(Body body)
        {
            prevIsAction = currentDrawable.Previous.GetType().IsSubclassOf(typeof(BaseAction));

            if (prevIsAction)
                isFulfilled = CheckCurrent(body);
            else
            {
                isFulfilled = currentDrawable.Previous.IsFulfilled(body);

                if (isFulfilled)
                {
                    isFulfilled = CheckCurrent(body);
                }
                else
                {
                    if (currentDrawable.GetType().IsSubclassOf(typeof(BaseAction)))
                        ((BaseAction)currentDrawable).Reset();

                    ((Drawable)currentDrawable.Previous).Write(body);
                    ((Drawable)currentDrawable.Previous).Draw(body);
                }
            }

            return isFulfilled;
        }

        public override bool IsFulfilled(Body body)
        {
            prevIsNotNull = currentDrawable.Previous != null;

            if(prevIsNotNull)
            {
                isFulfilled = CheckPrevious(body);
            }
            
            else
            {
                isFulfilled = CheckCurrent(body);
            }

            return isFulfilled;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}: {2}", this.GetType().Name, unityObjectName, isFulfilled);
        }
    }
}
