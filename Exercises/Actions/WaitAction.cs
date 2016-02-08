namespace HSA.RehaGame.Exercises.Actions
{
    using FulFillables;
    using DB;
    using UI.VisualExercise;
    using InGame;

    public class WaitAction : HoldAction
    {
        public WaitAction(string unityObjectName, double value, FulFillable previous, Database dbManager, Settings settings, Drawing drawing) : base(unityObjectName, value, previous, dbManager, settings, drawing)
        {

        }
    }
}
