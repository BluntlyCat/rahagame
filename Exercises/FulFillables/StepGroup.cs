namespace HSA.RehaGame.Exercises.FulFillables
{
    using System;
    using Windows.Kinect;

    public class StepGroup : BaseStep
    {
        public StepGroup(string description, BaseStep previous) : base(description, previous) {}

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = current.IsFulfilled(body);

            if (isFulfilled)
            {
                if (current.Next == null)
                {
                    isFulfilled = CheckActions(body);
                }
                else
                {
                    current = current.Next;
                    isFulfilled = false;
                }
            }

            return isFulfilled;
        }

        public override string Information()
        {
            return current.Information();
        }

        public override void VisualInformation(Body body)
        {
            current.VisualInformation(body);
        }

        public override void Debug(Body body)
        {
            current.Debug(body);
        }

        public Step CurrentStep
        {
            get
            {
                return current as Step;
            }
        }
    }
}
