namespace HSA.RehaGame.Exercises.FulFillables
{
    using System;
    using Windows.Kinect;

    public abstract class FulFillable : IFulFillable
    {
        protected Attributes attributes = new Attributes();
        protected string name;

        protected bool isFulfilled = false;

        protected FulFillable previous;
        protected FulFillable next;

        protected TimeSpan startTime;
        protected TimeSpan endTime;

        protected Types type = Types.fullfillable;

        public FulFillable(string name, FulFillable previous)
        {
            this.name = name;
            this.previous = previous;
        }

        public void AddNext(FulFillable next)
        {
            this.next = next;
        }

        public virtual FulFillable First
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

        public virtual FulFillable Previous
        {
            get
            {
                return previous;
            }
        }

        public virtual FulFillable Next
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

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Types Type
        {
            get
            {
                return this.type;
            }
        }

        public T GetAttribute<T>(string key) where T : class
        {
            return attributes.GetAttribute<T>(key);
        }

        public T Convert<T>() where T : FulFillable
        {
            return this as T;
        }

        public void AddAttribute<T>(string key, T attribute) where T : class
        {
            attributes.AddAttribute<T>(key, attribute);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} ({2})", this.GetType().Name, this.Name, isFulfilled);
        }

        public TimeSpan StartTime
        {
            get
            {
                return this.startTime;
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return this.endTime;
            }
        }

        public void SetStartTime()
        {
            this.startTime = DateTime.Now.TimeOfDay;
        }

        public void SetEndTime()
        {
            this.endTime = DateTime.Now.TimeOfDay;
        }

        public abstract bool IsFulfilled(Body body);
    }
}
