namespace HSA.RehaGame.Exercises.FulFillables
{
    using DB;
    using DB.Models;
    using Manager;
    using Manager.Audio;
    using System.Collections.Generic;
    using UI.Feedback;
    using Windows.Kinect;
    using Models = DB.Models;
    public abstract class Informable : FulFillable
    {
        protected string information = "";

        protected IDatabase database;
        protected SettingsManager settingsManager;
        protected Feedback feedback;
        protected PitchType pitchType;

        public Informable(SettingsManager settingsManager, Feedback feedback, PitchType pitchType, string name, StatisticType statisticType, PatientJoint affectedJoint, FulFillable previous, int repetitions, WriteStatisticManager statisticManager) : base (name, statisticType, affectedJoint, previous, repetitions, statisticManager)
        {
            this.database = Database.Instance();
            this.feedback = feedback;
            this.pitchType = pitchType;
        }

        public abstract void Write(Body body);

        public abstract void Draw(Body body);

        public abstract void PlayValue();

        public abstract void PlayFullfilledSound();

        public abstract void Clear();

        public abstract void Debug(Body body, IDictionary<string, Models.KinectJoint> stressedJoints);

        public abstract void Debug(Body body, Models.KinectJoint jointJoint);
    }
}
