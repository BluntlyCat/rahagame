namespace HSA.RehaGame.Exercises.FulFillables
{
    using System;
    using System.Collections.Generic;
    using Behaviours;
    using User;
    using UI.VisualExercise;
    using Windows.Kinect;

    public class Joint : Drawable
    {
        private BaseJointBehaviour firstJointBehaviour;
        private BaseJointBehaviour currentJointBehaviour;

        private string name;

        public Joint(string name, Drawing drawing, FulFillable previous) : base (drawing, previous)
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
            {
                currentJointBehaviour.Write(body);
                currentJointBehaviour.Draw(body);
            }

            return isFulfilled;
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

        public override void Write(Body body)
        {
            
        }

        public override void Draw(Body body)
        {
            
        }

        public override void Clear()
        {
            drawing.ClearDrawings();
        }

        public override void Debug(Body body, IDictionary<string, PatientJoint> stressedJoints)
        {
            throw new NotImplementedException();
        }

        public override void Debug(Body body, PatientJoint jointJoint)
        {
            throw new NotImplementedException();
        }
    }
}
