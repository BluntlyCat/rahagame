namespace HSA.RehaGame.Exercises.FulFillables
{
    using Logging;

    public class Step : BaseStep
    {
        private static Logger<Step> logger = new Logger<Step>();

        

        public Step(string name, BaseStep prevStep) : base(name, prevStep)
        {
            logger.AddLogAppender<ConsoleAppender>();
            this.type = Types.step;
        }

        public void SetFirstInformable(Informable firstToDoAble)
        {
            this.firstInformable = this.currentInformable = firstToDoAble;
        }
    }
}
