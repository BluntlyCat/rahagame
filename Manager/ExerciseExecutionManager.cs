namespace HSA.RehaGame.Manager
{
    using Audio;
    using Exercises.FulFillables;
    using Logging;
    using System;
    using System.Collections.Generic;
    using UI.Feedback;
    using Windows.Kinect;
    using Models = DB.Models;
    public class ExerciseExecutionManager : Informable
    {
        private static Logger<ExerciseExecutionManager> logger = new Logger<ExerciseExecutionManager>();
        private FulFillable currentFulFillable;

        public ExerciseExecutionManager(BaseStep firstFulFillable, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, string name, FulFillable previous) : base(settingsManager, feedback, pitchType, name, previous)
        {
            logger.AddLogAppender<ConsoleAppender>();

            this.currentFulFillable = firstFulFillable;
        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = currentFulFillable.IsFulfilled(body);

            if (isFulfilled)
            {
                if (currentFulFillable.Next == null)
                {
                    currentFulFillable.SetEndTime();
                    isFulfilled = true;
                }
                else
                {
                    currentFulFillable.SetStartTime();
                    feedback.PitchFullfilledSound();
                    currentFulFillable = currentFulFillable.Next as FulFillable;
                    currentFulFillable.SetEndTime();

                    isFulfilled = false;
                }
            }

            return isFulfilled;
        }

        public override void Draw(Body body)
        {
            if (currentFulFillable.GetType() == typeof(Informable))
                ((Informable)currentFulFillable).Draw(body);
        }

        public override void PlayValue()
        {
            if (currentFulFillable.GetType() == typeof(Informable))
                ((Informable)currentFulFillable).PlayValue();
        }

        public override void PlayFullfilledSound()
        {
            feedback.PitchFullfilledSound();
        }

        public override void Clear()
        {
            if (currentFulFillable.GetType() == typeof(Informable))
                ((Informable)currentFulFillable).Clear();
        }

        public override void Write(Body body)
        {
            if (currentFulFillable.GetType() == typeof(Informable))
                ((Informable)currentFulFillable).Write(body);
        }

        public override void Debug(Body body, Models.KinectJoint joint)
        {
            
        }

        public override void Debug(Body body, IDictionary<string, Models.KinectJoint> stressedJoints)
        {
            
        }

        public Dictionary<string, TimeSpan> GetExecutionTimes()
        {
            var first = currentFulFillable.First;
            var last = currentFulFillable.Last;
            var current = first;

            Dictionary<string, TimeSpan> times = new Dictionary<string, TimeSpan>();

            times.Add("accumulatedTimes", last.EndTime - first.StartTime);

            do
            {
                times.Add(current.Name, current.EndTime - current.StartTime);
                current = current.Next;
            }
            while (current != null);

            return times;
        }
    }
}
