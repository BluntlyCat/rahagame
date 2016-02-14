namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB;
    using DB.Models;
    using FulFillables;
    using Logging;
    using UI.VisualExercise;
    using Windows.Kinect;

    public class AboveBehaviour : BaseJointBehaviour
    {
        private static Logger<AboveBehaviour> logger = new Logger<AboveBehaviour>();

        public AboveBehaviour(string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base(unityObjectName, activeJoint, passiveJoint, dbManager, settings, drawing, previous)
        {
            logger.AddLogAppender<ConsoleAppender>();
        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = body.Joints[activeJoint.Type].Position.Y > body.Joints[passiveJoint.Type].Position.Y;
            return isFulfilled;
        }

        public override void Draw(Body body)
        {
            
        }

        public override void Clear()
        {
            drawing.ClearDrawings();
        }
    }
}
