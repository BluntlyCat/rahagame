namespace HSA.RehaGame.Exercises.FulFillables
{
    using Windows.Kinect;

    public abstract class FulFillable : IFulFillable
    {
        protected Attributes attributes = new Attributes();
        protected bool isFulfilled = false;

        public abstract bool IsFulfilled(Body body);

        public T GetAttribute<T>(string key) where T : class
        {
            return attributes.GetAttribute<T>(key);
        }

        public void AddAttribute<T>(string key, T attribute) where T : class
        {
            attributes.AddAttribute<T>(key, attribute);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.GetType().Name, isFulfilled);
        }
    }
}
