namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB.Models;
    using FulFillables;
    using Manager;
    using Manager.Audio;
    using Math;
    using System.Collections.Generic;
    using System.Linq;
    using UI.Feedback;
    using Windows.Kinect;
    using Models = DB.Models;
    public abstract class BaseJointBehaviour : Informable
    {   
        protected PatientJoint activeJoint;
        protected PatientJoint passiveJoint;

        public BaseJointBehaviour(string unityObjectName, StatisticType statisticType, PatientJoint affectedJoint, PatientJoint activeJoint, Models.PatientJoint passiveJoint, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base(settingsManager, feedback, pitchType, unityObjectName, statisticType, affectedJoint, previous, repetitions, statisticManager)
        {
            this.type = Types.behaviour;
            this.activeJoint = activeJoint;
            this.passiveJoint = passiveJoint;

            this.information = Models.Model.GetModel<Models.ExerciseInformation>(unityObjectName).Order;
        }

        public abstract override bool IsFulfilled(Body body);

        public override void PlayFullfilledSound()
        {
            feedback.PitchFullfilledSound();
        }

        public virtual double GetAngle(Body body)
        {
            return Calculations.GetAngle(
                body.Joints[activeJoint.KinectJoint.Type], body.Joints[activeJoint.KinectJoint.Parent.Type], body.Joints[activeJoint.KinectJoint.Children.First().Value.Type]
            );
        }

        public override void Draw(Body body)
        {
            feedback.DrawCircle(360d, this.GetAngle(body), this.activeJoint.KinectJoint);
        }

        public abstract override void PlayValue();

        public override void Write(Body body)
        {
            feedback.ShowInformation(string.Format(information, activeJoint.KinectJoint.Translation, passiveJoint.KinectJoint.Translation));
        }

        public override void Debug(Body body, IDictionary<string, Models.KinectJoint> stressedJoints)
        {
            feedback.DrawDebug(body, stressedJoints);
        }

        public override void Debug(Body body, Models.KinectJoint joint)
        {
            feedback.DrawDebug(body, joint);
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
