namespace HSA.RehaGame.Exercises.Actions
{
    using DB;
    using FulFillables;
    using Windows.Kinect;

    public abstract class BaseAction : Informable
    {
        private string unityObjectName;
        protected double value;
        protected double initialValue;

        public BaseAction(string unityObjectName, double value)
        {
            this.unityObjectName = unityObjectName;
            this.initialValue = this.value = value;
            this.information = DBManager.GetAction(unityObjectName).GetValueFromLanguage("order");
        }

        public override string Information()
        {
            return string.Format(information, value.ToString("0"));
        }

        public override void Debug(Body body)
        {
            return;
        }

        public abstract override bool IsFulfilled(Body body);

        public abstract void Reset();
    }
}
