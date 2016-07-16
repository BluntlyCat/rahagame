namespace HSA.RehaGame.Exercises.FulFillables
{
    using Actions;
    using DB.Models;
    using Manager;
    using Windows.Kinect;

    public abstract class BaseStep : FulFillable
    {
        protected Informable firstInformable;
        protected Informable currentInformable;

        protected bool prevIsNull;
        protected string unityObjectName;

        public BaseStep(string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base(unityObjectName, statisticType, affectedJoint, previous, repetitions, statisticManager)
        {
            this.unityObjectName = unityObjectName;
        }

        private bool CheckCurrent(Body body)
        {
            isFulfilled = currentInformable.IsFulfilled(body);

            if (isFulfilled)
            {
                currentInformable.PlayFullfilledSound();
                currentInformable.Fulfilled();

                if (currentInformable.Next == null)
                {
                    isFulfilled = true;
                }
                else
                {
                    currentInformable = currentInformable.Next.Convert<Informable>();
                    currentInformable.Unfulfilled();

                    if (currentInformable.Type == Types.joint)
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

                currentInformable.Previous.Canceled();
                currentInformable.Canceled();
                currentInformable = currentInformable.Previous.Convert<Informable>();
                currentInformable.Write(body);
                currentInformable.Draw(body);
            }

            return isFulfilled;
        }

        public override bool IsFulfilled(Body body)
        {
            prevIsNull = currentInformable.Previous == null;

            if (prevIsNull)
            {
                isFulfilled = CheckCurrent(body);
            }

            else
            {
                isFulfilled = CheckPrevious(body);
            }

            return isFulfilled;
        }
    }
}
