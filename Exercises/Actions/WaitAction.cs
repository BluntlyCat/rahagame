namespace HSA.RehaGame.Exercises.Actions
{
    using FulFillables;
    using UI.VisualExercise;

    public class WaitAction : HoldAction
    {
        public WaitAction(string unityObjectName, double value, FulFillable previous, Drawing drawing) : base(unityObjectName, value, previous, drawing)
        {

        }
    }
}
