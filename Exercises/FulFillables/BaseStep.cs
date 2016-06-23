namespace HSA.RehaGame.Exercises.FulFillables
{
    using Actions;
    using Windows.Kinect;

    public abstract class BaseStep : FulFillable
    {
        protected Informable firstInformable;
        protected Informable currentInformable;

        protected bool prevIsAction;
        protected bool prevIsNotNull;

        protected string unityObjectName;

        public BaseStep(string unityObjectName, FulFillable previous) : base (unityObjectName, previous)
        {
            this.unityObjectName = unityObjectName;
        }

        private bool CheckCurrent(Body body)
        {
            isFulfilled = currentInformable.IsFulfilled(body);

            if (isFulfilled)
            {
                currentInformable.PlayFullfilledSound();

                if (currentInformable.Next == null)
                {
                    isFulfilled = true;
                }
                else
                {
                    currentInformable = currentInformable.Next.Convert<Informable>();

                    if(currentInformable.Type == Types.joint)
                        currentInformable.Convert<Joint>().SetPreviousAsInactive();

                    isFulfilled = false;
                }
            }
            else
            {
                currentInformable.Write(body);
                currentInformable.Draw(body);
                currentInformable.PlayValue();
            }

            return isFulfilled;
        }

        private bool CheckPrevious(Body body)
        {
            //if (currentInformable.Type == Types.action)
              //  isFulfilled = CheckCurrent(body);
            //else
            {
                isFulfilled = currentInformable.Previous.IsFulfilled(body);

                if (isFulfilled)
                {
                    isFulfilled = CheckCurrent(body);
                }
                else
                {
                    if (currentInformable.Type == Types.action)
                        currentInformable.Convert<BaseAction>().Reset();

                    else if (currentInformable.Type == Types.joint)
                        currentInformable.Convert<Joint>().SetPreviousAsActive();

                    currentInformable = currentInformable.Previous.Convert<Informable>();
                    currentInformable.Write(body);
                    currentInformable.Draw(body);
                }
            }

            return isFulfilled;
        }

        public override bool IsFulfilled(Body body)
        {
            prevIsNotNull = currentInformable.Previous != null;

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
