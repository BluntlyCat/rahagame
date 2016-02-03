namespace HSA.RehaGame.Exercises.FulFillables
{
    using Logging;

    public class Step : BaseStep
    {
        private static Logger<Step> logger = new Logger<Step>();

        public Step(string name, BaseStep prevStep) : base(name, prevStep)
        {
            logger.AddLogAppender<ConsoleAppender>();
        }

        public void SetFirstDrawable(Drawable firstToDoAble)
        {
            this.firstDrawable = this.currentDrawable = firstToDoAble;
        }
    }
}
