namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using Actions;
    using Logging;
    using User;
    using Windows.Kinect;

    public class Step : BaseStep
    {
        private static Logger<Step> logger = new Logger<Step>();

        public Step(string name, BaseStep prevStep) : base(name, prevStep)
        {
            logger.AddLogAppender<ConsoleAppender>();
        }

        public override string Information()
        {

            if (currentToDoAble.GetType().IsSubclassOf(typeof(BaseAction)) && currentToDoAble.Previous != null && !currentToDoAble.Previous.Fullfilled)
                return ((Informable)currentToDoAble.Previous).Information();

            return currentToDoAble.Information();    
        }

        public override void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            currentToDoAble.Debug(body, stressedJoints);
        }

        public override void Debug(Body body, PatientJoint patintJoint)
        {
            currentToDoAble.Debug(body, patintJoint);
        }

        public void SetFirstToDoAble(Informable firstToDoAble)
        {
            this.firstToDoAble = this.currentToDoAble = firstToDoAble;
        }

        public Informable CurrentTodoAble
        {
            get
            {
                return currentToDoAble;
            }
        }
    }
}
