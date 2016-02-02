namespace HSA.RehaGame.Exercises.FulFillables
{
    using System.Collections.Generic;
    using Behaviours;
    using User;
    using Windows.Kinect;

    public class Joint : Informable
    {
        private BaseJointBehaviour firstJointBehaviour;
        private BaseJointBehaviour currentJointBehaviour;

        private string name;

        public Joint(string name, FulFillable previous) : base (previous)
        {
            this.name = name;
        }

        public void SetFirstBehaviour(BaseJointBehaviour behaviour)
        {
            this.firstJointBehaviour = this.currentJointBehaviour = behaviour;
        }

        public override bool IsFulfilled(Body body)
        {
            // ToDo Was ist wenn man die Position vom vorherigen Schritt verlässt?
            isFulfilled = currentJointBehaviour.IsFulfilled(body);

            if (isFulfilled)
            {
                currentJointBehaviour.Clear();

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
            else
                currentJointBehaviour.Draw(body);

            return isFulfilled;
        }

        public override string Information()
        {
            return currentJointBehaviour.Information();
        }

        public override void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            currentJointBehaviour.Debug(body, stressedJoints);
        }

        public override void Debug(Body body, PatientJoint patientJoint)
        {
            currentJointBehaviour.Debug(body, patientJoint);
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", name, isFulfilled);
        }
    }
}
