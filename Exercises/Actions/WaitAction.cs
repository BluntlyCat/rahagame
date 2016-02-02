namespace HSA.RehaGame.Exercises.Actions
{
    using FulFillables;

    public class WaitAction : HoldAction
    {
        public WaitAction(string unityObjectName, double value, FulFillable previous) : base(unityObjectName, value, previous)
        {

        }
    }
}
