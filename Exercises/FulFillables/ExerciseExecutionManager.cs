namespace HSA.RehaGame.Exercises.FulFillables
{
    using System;
    using User;
    using Logging;
    using Windows.Kinect;

    public class ExerciseExecutionManager : Informable
    {
        private static Logger<ExerciseExecutionManager> logger = new Logger<ExerciseExecutionManager>();
        private BaseStep currentFulFillable;

        public ExerciseExecutionManager(BaseStep firstFulFillable)
        {
            logger.AddLogAppender<ConsoleAppender>();
            this.currentFulFillable = firstFulFillable;
        }

        public override bool IsFulfilled(Body body)
        {
#if UNITY_EDITOR
            //this.Debug(body);
#endif
            isFulfilled = currentFulFillable.IsFulfilled(body);

            if (isFulfilled)
            {
                if (currentFulFillable.Next == null)
                {
                    isFulfilled = true;
                }
                else
                {
                    currentFulFillable = currentFulFillable.Next;
                    isFulfilled = false;
                }
            }

            return isFulfilled;
        }

        public override string Information()
        {
            return currentFulFillable.Information();
        }

        public override void VisualInformation(Body body)
        {
            currentFulFillable.VisualInformation(body);
        }

        public override void Debug(Body body)
        {
            currentFulFillable.Debug(body);
        }

        public Step CurrentFulFillable
        {
            get
            {
                if (currentFulFillable.GetType() == typeof(StepGroup))
                    return ((StepGroup)currentFulFillable).Current as Step;

                return currentFulFillable as Step;
            }
        }
    }
}
