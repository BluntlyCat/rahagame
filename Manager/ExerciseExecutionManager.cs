namespace HSA.RehaGame.Manager
{
    using System.Collections.Generic;
    using DB;
    using Exercises.FulFillables;
    using Logging;
    using UI = UI.VisualExercise;
    using Windows.Kinect;
    using Models = DB.Models;

    public class ExerciseExecutionManager : Drawable
    {
        private static Logger<ExerciseExecutionManager> logger = new Logger<ExerciseExecutionManager>();
        private FulFillable currentFulFillable;
        private IDictionary<string, Models.Joint> stressedJoints;

        public ExerciseExecutionManager(BaseStep firstFulFillable, IDictionary<string, Models.Joint> stressedJoints, Database dbManager, Models.Settings settings, UI.Drawing drawing, FulFillable previous) : base(dbManager, settings, drawing, previous)
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

        public override void Debug(Body body, Models.Joint joint)
        {
            
        }

        public override void Debug(Body body, IDictionary<string, Models.Joint> stressedJoints)
        {
            
        }
    }
}
