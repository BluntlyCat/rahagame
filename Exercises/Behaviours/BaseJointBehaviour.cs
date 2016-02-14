namespace HSA.RehaGame.Exercises.Behaviours
{
    using System.Collections.Generic;
    using DB;
    using FulFillables;
    using Windows.Kinect;
    using Models = DB.Models;
    using UI = UI.VisualExercise;

    public abstract class BaseJointBehaviour : Drawable
    {   
        protected Models.PatientJoint activeJoint;
        protected Models.PatientJoint passiveJoint;

        public BaseJointBehaviour(string unityObjectName, Models.PatientJoint activeJoint, Models.PatientJoint passiveJoint, Database dbManager, Models.Settings settings, UI.Drawing drawing, FulFillable previous) : base(dbManager, settings, drawing, previous)
        {
            this.activeJoint = activeJoint;
            this.passiveJoint = passiveJoint;

            this.information = Models.Model.GetModel<Models.ExerciseInformation>(unityObjectName).Order;
        }

        public abstract override bool IsFulfilled(Body body);

        public override void Draw(Body body)
        {
            
        }

        public override void Write(Body body)
        {
            drawing.ShowInformation(string.Format(information, activeJoint.Translation, passiveJoint.Translation));
        }

        public override void Debug(Body body, IDictionary<string, Models.Joint> stressedJoints)
        {
            drawing.DrawDebug(body, stressedJoints);
        }

        public override void Debug(Body body, Models.Joint joint)
        {
            drawing.DrawDebug(body, joint);
        }

        public Models.PatientJoint ActiveJoint
        {
            get
            {
                return activeJoint;
            }
        }

        public Models.PatientJoint PassiveJoint
        {
            get
            {
                return passiveJoint;
            }
        }
    }
}
