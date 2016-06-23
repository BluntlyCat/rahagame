namespace HSA.RehaGame.Exercises.FulFillables
{
    using Behaviours;
    using Manager;
    using Manager.Audio;
    using System;
    using System.Collections.Generic;
    using UI.Feedback;
    using Windows.Kinect;
    using Models = DB.Models;

    public class Joint : Informable
    {
        private BaseJointBehaviour currentJointBehaviour;

        public Joint(string name, SettingsManager settingsManager, Feedback feedback, PitchType pitchType, FulFillable previous) : base (settingsManager, feedback, pitchType, name, previous)
        {
            this.type = Types.joint;
        }

        public void SetFirstBehaviour(BaseJointBehaviour behaviour)
        {
            this.currentJointBehaviour = behaviour;
        }

        public override bool IsFulfilled(Body body)
        {
            isFulfilled = currentJointBehaviour.IsFulfilled(body);
            currentJointBehaviour.ActiveJoint.KinectJoint.ActiveInExercise = true;

            if (isFulfilled)
            {
                currentJointBehaviour.Clear();
                currentJointBehaviour.ActiveJoint.KinectJoint.ActiveInExercise = false;

                if (currentJointBehaviour.Next == null)
                {
                    isFulfilled = true;
                }
                else
                {
                    currentJointBehaviour = currentJointBehaviour.Next as BaseJointBehaviour;
                    isFulfilled = false;
                }
            }

            return isFulfilled;
        }

        public void SetPreviousAsActive()
        {
            if (currentJointBehaviour.Previous != null && currentJointBehaviour.Previous.Type == Types.behaviour)
                currentJointBehaviour.Previous.Convert<BaseJointBehaviour>().ActiveJoint.KinectJoint.ActiveInExercise = true;

            currentJointBehaviour.ActiveJoint.KinectJoint.ActiveInExercise = false;
        }

        public void SetPreviousAsInactive()
        {
            if (currentJointBehaviour.Previous != null && currentJointBehaviour.Previous.Type == Types.behaviour)
            {
                var previous = currentJointBehaviour.Previous.Convert<BaseJointBehaviour>();
                previous.ActiveJoint.KinectJoint.ActiveInExercise = false;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.Name, isFulfilled);
        }

        public override void Write(Body body)
        {
            currentJointBehaviour.Write(body);
        }

        public override void PlayValue()
        {
            currentJointBehaviour.PlayValue();
        }

        public override void PlayFullfilledSound()
        {
            feedback.PitchFullfilledSound();
        }

        public override void Draw(Body body)
        {
            currentJointBehaviour.Draw(body);
        }

        public override void Clear()
        {
            feedback.ClearDrawings();
        }

        public override void Debug(Body body, IDictionary<string, Models.KinectJoint> stressedJoints)
        {
            throw new NotImplementedException();
        }

        public override void Debug(Body body, Models.KinectJoint joint)
        {
            throw new NotImplementedException();
        }
    }
}
