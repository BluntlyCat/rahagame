namespace HSA.RehaGame.Exercises.FulFillables
{
    using Windows.Kinect;

    public abstract class FulFillable : IFulFillable
    {
        protected Attributes attributes = new Attributes();

        protected bool isFulfilled = false;

        protected FulFillable previous;
        protected FulFillable next;

        public FulFillable(FulFillable previous)
        {
            this.previous = previous;
        }

        public void AddNext(FulFillable next)
        {
            this.next = next;
        }

        public FulFillable First
        {
            get
            {
                var current = this;

                while (current.previous != null)
                    current = current.previous;

                return current;
            }
        }

        public FulFillable Last
        {
            get
            {
                var current = this;

                while (current.next != null)
                    current = current.next;

                return current;
            }
        }

        public FulFillable Previous
        {
            get
            {
                return previous;
            }
        }

        public FulFillable Next
        {
            get
            {
                return next;
            }
        }

        public bool Fullfilled
        {
            get
            {
                return isFulfilled;
            }
        }

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

        public abstract bool IsFulfilled(Body body);
    }
}
