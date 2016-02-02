namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using Logging;
    using User;
    using Windows.Kinect;

    public class ExerciseExecutionManager : Informable
    {
        private static Logger<ExerciseExecutionManager> logger = new Logger<ExerciseExecutionManager>();
        private Informable currentFulFillable;
        private IDictionary<string, PatientJoint> stressedJoints;

        public ExerciseExecutionManager(BaseStep firstFulFillable, IDictionary<string, PatientJoint> stressedJoints, FulFillable previous) : base(previous)
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.currentFulFillable = firstFulFillable;
            this.stressedJoints = stressedJoints;
        }

        public override bool IsFulfilled(Body body)
        {
#if UNITY_EDITOR
            this.Debug(body, stressedJoints);
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
                    currentFulFillable = currentFulFillable.Next as Informable;
                    isFulfilled = false;
                }
            }

            return isFulfilled;
        }

        public override string Information()
        {
            return currentFulFillable.Information();
        }

        public override void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            currentFulFillable.Debug(body, stressedJoints);
        }

        public override void Debug(Body body, PatientJoint jointJoint)
        {
            currentFulFillable.Debug(body, jointJoint);
        }
    }
}
