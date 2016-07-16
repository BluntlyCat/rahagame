namespace HSA.RehaGame.Exercises.FulFillables
{
    using DB.Models;
    using Manager;
    using Windows.Kinect;

    public class Exercise : FulFillable
    {
        private BaseStep currentFulFillable;

        public Exercise(string name, StatisticType statisticType, PatientJoint affectedJoint, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base(name, statisticType, affectedJoint, previous, repetitions, statisticManager)
        {
        }

        public void SetFirstStep(Step firstStep)
        {
            this.currentFulFillable = firstStep;
        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = currentFulFillable.IsFulfilled(body);

            if (isFulfilled)
            {
                currentFulFillable.Fulfilled();

                if (currentFulFillable.Next == null)
                {
                    isFulfilled = true;
                }
                else
                {
                    currentFulFillable = currentFulFillable.Next as BaseStep;
                    currentFulFillable.Unfulfilled();

                    isFulfilled = false;
                }
            }

            return isFulfilled;
        }
    }
}
