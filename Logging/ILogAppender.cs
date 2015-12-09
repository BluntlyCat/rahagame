namespace HSA.RehaGame.Logging
{
    public interface ILogAppender
    {
        void Append(string mesage, LogLevels level);
    }
}
