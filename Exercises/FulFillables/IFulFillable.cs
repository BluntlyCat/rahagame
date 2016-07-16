namespace HSA.RehaGame.Exercises.FulFillables
{
    using Windows.Kinect;

    public interface IFulFillable
    {
        bool IsFulfilled(Body body);

        void Reset();

        T GetAttribute<T>(string name) where T : class;

        T Convert<T>() where T : FulFillable;

        void AddAttribute<T>(string key, T attribute) where T : class;

        void Unfulfilled();

        void Fulfilled();

        void Canceled();
    }
}
