namespace HSA.RehaGame.Exercises.Behaviours
{
    using DB;
    using DB.Models;
    using FulFillables;
    using Math;
    using UI.VisualExercise;
    using UnityEngine;
    using Windows.Kinect;

    public class DistanceValueBehaviour : BaseJointValueBehaviour
    {
        private Vector3 distance;

        public DistanceValueBehaviour(double value, string unityObjectName, PatientJoint activeJoint, PatientJoint passiveJoint, Database dbManager, Settings settings, Drawing drawing, FulFillable previous) : base(value, unityObjectName, activeJoint, passiveJoint, dbManager, settings, drawing, previous)
        {

        }

        public override bool IsFulfilled(Body body)
        {
            var active = body.Joints[activeJoint.Type];
            var passive = body.Joints[passiveJoint.Type];

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
            drawing.DrawLine(body.Joints[activeJoint.Type], body.Joints[passiveJoint.Type], 1f);
        }

        public override void Clear()
        {
            drawing.ClearDrawings();
        }
    }
}
