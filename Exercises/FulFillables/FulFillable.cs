namespace HSA.RehaGame.Exercises.FulFillables
{
    using DB.Models;
    using Logging;
    using Manager;
    using System;
    using Windows.Kinect;

    public abstract class FulFillable : IFulFillable
    {
        private WriteStatisticManager statisticManager;
        private StatisticType dataType;
        private PatientJoint affectedJoint;

        protected StatisticData statisticData;

        protected Logger<FulFillable> logger = new Logger<FulFillable>();

        protected Attributes attributes = new Attributes();

        protected string name;

        protected bool isFulfilled = false;

        protected FulFillable previous;
        protected FulFillable next;

        protected Types type = Types.fullfillable;

        protected int initialRepetitions;

        protected int repetitions;

        public FulFillable(string name, StatisticType dataType, PatientJoint affectedJoint, FulFillable previous, int repetitions, WriteStatisticManager statisticManager)
        {
            this.logger.AddLogAppender<ConsoleAppender>();

            this.name = name;
            this.previous = previous;
            this.repetitions = this.initialRepetitions = repetitions;
            this.statisticManager = statisticManager;
            this.dataType = dataType;
            this.affectedJoint = affectedJoint;

            this.statisticData = AddStatisticData(dataType, affectedJoint);
        }

        private StatisticData AddStatisticData(StatisticType dataType, PatientJoint affectedJoint)
        {
            return statisticManager.AddStatistic(name, dataType, StatisticStates.unfulfilled, "", affectedJoint);
        }

        public void Unfulfilled()
        {
            statisticData.State = StatisticStates.unfulfilled;
        }

        public void Fulfilled()
        {
            statisticData.State = StatisticStates.finished;
            statisticData.EndTime = DateTime.Now;
        }

        public void Canceled()
        {
            statisticData.State = StatisticStates.canceled;
            statisticData.EndTime = DateTime.Now;

            this.statisticData = this.AddStatisticData(this.dataType, this.affectedJoint);
        }

        public virtual bool IsFulfilled(Body body)
        {
            return this.repetitions == 0;
        }

        public virtual void Reset()
        {
            this.repetitions = this.initialRepetitions;
            this.isFulfilled = false;
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
    }
}
