namespace HSA.RehaGame.Exercises.Actions
{
    using DB;
    using DB.Models;
    using FulFillables;
    using UI.VisualExercise;
    using Windows.Kinect;

    public class RepeatAction : BaseAction
    {
        public RepeatAction(string unityObjectName, double value, FulFillable previous, Database dbManager, Settings settings, Drawing drawing) : base(unityObjectName, value, previous, dbManager, settings, drawing)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = value == 0;

            return isFulfilled;
        }

        public override void Reset()
        {
            isFulfilled = false;
        }
    }
}
