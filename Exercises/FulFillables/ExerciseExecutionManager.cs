namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using UI.VisualExercise;
    using Logging;
    using User;
    using Windows.Kinect;
    using System;

    public class ExerciseExecutionManager : Drawable
    {
        private static Logger<ExerciseExecutionManager> logger = new Logger<ExerciseExecutionManager>();
        private FulFillable currentFulFillable;
        private IDictionary<string, PatientJoint> stressedJoints;

        public ExerciseExecutionManager(BaseStep firstFulFillable, IDictionary<string, PatientJoint> stressedJoints, Drawing drawing, FulFillable previous) : base(drawing, previous)
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
                    currentFulFillable = currentFulFillable.Next as FulFillable;
                    isFulfilled = false;
                }
            }

            return isFulfilled;
        }

        public override void Draw(Body body)
        {
            
        }

        public override void Clear()
        {
            
        }

        public override void Write(Body body)
        {
            
        }

        public override void Debug(Body body, PatientJoint jointJoint)
        {
            
        }

        public override void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            
        }
    }
}
