namespace HSA.RehaGame.Exercises.Actions
{
    using DB;
    using DB.Models;
    using FulFillables;
    using UI.VisualExercise;

    public class WaitAction : HoldAction
    {
        public WaitAction(string unityObjectName, double value, FulFillable previous, Database dbManager, Settings settings, Drawing drawing) : base(unityObjectName, value, previous, dbManager, settings, drawing)
        {

        }
    }
}
