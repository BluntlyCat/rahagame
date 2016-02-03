namespace HSA.RehaGame.Input.Kinect
{
    using System.Collections.Generic;
    using InGame;
    using Logging;
    using Math;
    using UnityEngine;
    using User;
    using Kinect = Windows.Kinect;

    public class BodyManagerView : MonoBehaviour
    {
        private static Logger<BodyManager> logger = new Logger<BodyManager>();

        public Material boneMaterial;
        public Material jointMaterial;
        public Material stressedJointMaterial;
        public Material disabledJointMaterial;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
        }

        public GameObject CreateBodyObject(ulong id)
        {
            GameObject body = new GameObject(GameState.ActivePatient.Name);

            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                PatientJoint patientJoint = GameState.ActivePatient.GetJoint(jt);
                LineRenderer lr = jointObj.AddComponent<LineRenderer>();
                Renderer jointRenderer = jointObj.GetComponent<Renderer>();

                if (patientJoint.Active == false)
                    jointRenderer.material = disabledJointMaterial;
                else if (patientJoint.Stressed)
                    jointRenderer.material = stressedJointMaterial;
                else
                    jointRenderer.material = jointMaterial;

                lr.SetVertexCount(2);
                lr.material = boneMaterial;
                lr.SetWidth(0.05f, 0.05f);

                jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                jointObj.name = jt.ToString();
                jointObj.transform.parent = body.transform;
            }

            return body;
        }

        public void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
        {
            if (GameState.ExerciseIsActive)
            {
                for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
                {
                    var joint = GameState.ActivePatient.GetJoint(jt);

                    Kinect.Joint sourceJoint = body.Joints[jt];
                    Kinect.Joint? targetJoint = null;

                    if (joint.Parent != null)
                        targetJoint = body.Joints[joint.Parent.JointType];
                    else
                        targetJoint = body.Joints[joint.JointType];

                    Transform jointObject = bodyObject.transform.FindChild(jt.ToString());
                    jointObject.localPosition = Calculations.GetVector3FromJoint(sourceJoint);

                    LineRenderer lr = jointObject.GetComponent<LineRenderer>();

                    if (targetJoint.HasValue)
                    {
                        lr.SetPosition(0, jointObject.localPosition);
                        lr.SetPosition(1, Calculations.GetVector3FromJoint(targetJoint.Value));
                        lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
                    }
                    else
                    {
                        lr.enabled = false;
                    }
                }
            }
        }

        private static Color GetColorForState(Kinect.TrackingState state)
        {
            switch (state)
            {
                case Kinect.TrackingState.Tracked:
                    return Color.green;

                case Kinect.TrackingState.Inferred:
                    return Color.red;

                default:
                    return Color.black;
            }
        }
    }
}