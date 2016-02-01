namespace HSA.RehaGame.Exercises.FulFillables
{
    using Windows.Kinect;

    public interface IFulFillable
    {
        bool IsFulfilled(Body body);

        T GetAttribute<T>(string name) where T : class;

        void AddAttribute<T>(string key, T attribute) where T : class;
    }
}
