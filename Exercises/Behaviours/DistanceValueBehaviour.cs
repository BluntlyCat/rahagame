namespace HSA.RehaGame.Exercises.Behaviours
{
    using UI.VisualExercise;
    using User;
    using Windows.Kinect;
    using Math;
    using UnityEngine;
    using System;
    using FulFillables;
    using InGame;
    using DB;

    public class DistanceValueBehaviour : BaseJointValueBehaviour
    {
        private Vector3 distance;

        public DistanceValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base(value, unityObjectName, activeJoint, passiveJoint, dbManager, settings, drawing, previous)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            var active = body.Joints[activeJoint.JointType];
            var passive = body.Joints[passiveJoint.JointType];

            distance = Calculations.GetDistance(active, passive);

            isFulfilled = distance.x <= value && distance.y <= value && distance.z <= value;

            return isFulfilled;
        }

        public override void Write(Body body)
        {
            drawing.ShowInformation(string.Format(information, activeJoint.Translation, passiveJoint.Translation, value));
        }

        public override void Draw(Body body)
        {
            drawing.DrawLine(body.Joints[activeJoint.JointType], body.Joints[passiveJoint.JointType], 1f);
        }

        public override void Clear()
        {
            drawing.ClearDrawings();
        }
    }
}
