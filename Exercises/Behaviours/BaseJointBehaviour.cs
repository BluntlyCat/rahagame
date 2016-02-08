namespace HSA.RehaGame.Exercises.Behaviours
{
    using System;
    using System.Collections.Generic;
    using DB;
    using FulFillables;
    using UI = UI.VisualExercise;
    using User;
    using Windows.Kinect;
    using InGame;

    public abstract class BaseJointBehaviour : Drawable
    {   
        protected PatientJoint activeJoint;
        protected PatientJoint passiveJoint;

        public BaseJointBehaviour(string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, UI.Drawing drawing, FulFillable previous) : base(dbManager, settings, drawing, previous)
        {
            this.activeJoint = activeJoint;
            this.passiveJoint = passiveJoint;

            this.information = dbManager.GetExerciseInformation(unityObjectName, "behaviour").GetValueFromLanguage("order");
        }

        public abstract override bool IsFulfilled(Body body);

        public override void Draw(Body body)
        {
            
        }

        public override void Write(Body body)
        {
            drawing.ShowInformation(string.Format(information, activeJoint.Translation, passiveJoint.Translation));
        }

        public override void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            drawing.DrawDebug(body, stressedJoints);
        }

        public override void Debug(Body body, PatientJoint jointJoint)
        {
            drawing.DrawDebug(body, jointJoint);
        }

        public PatientJoint ActiveJoint
        {
            get
            {
                return activeJoint;
            }
        }

        public PatientJoint PassiveJoint
        {
            get
            {
                return passiveJoint;
            }
        }
    }
}
