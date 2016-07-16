namespace HSA.RehaGame.Input.Kinect
{
    using DB.Models;
    using Logging;
    using Manager;
    using Math;
    using UnityEngine;
    using Kinect = Windows.Kinect;

    public class BodyManagerView : MonoBehaviour
    {
        private static Logger<BodyManager> logger = new Logger<BodyManager>();

        public GameManager gameManager;

        public Material defaultBoneMaterial;
        public Material activeBoneInExerciseMaterial;
        public Material jointMaterial;
        public Material stressedJointMaterial;
        public Material disabledJointMaterial;
        public Material activeInExerciseMaterial;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
        }

        public GameObject CreateBodyObject(ulong id)
        {
            GameObject body = new GameObject(GameManager.ActivePatient.Name);
            Material boneMaterial = defaultBoneMaterial;

            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                PatientJoint patientJoint = GameManager.ActivePatient.GetJointByName(jt.ToString());
                LineRenderer lr = jointObj.AddComponent<LineRenderer>();
                Renderer jointRenderer = jointObj.GetComponent<Renderer>();

                if(patientJoint.Active == false)
                    jointRenderer.material = disabledJointMaterial;
                else if (patientJoint.KinectJoint.Stressed)
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
            if (GameManager.ExerciseIsActive)
            {
                for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
                {
                    var joint = GameManager.ActivePatient.GetJointByName(jt.ToString());

                    Kinect.Joint sourceJoint = body.Joints[jt];
                    Kinect.Joint? targetJoint = null;

                    if (joint.KinectJoint.Parent != null)
                        targetJoint = body.Joints[joint.KinectJoint.Parent.Type];
                    else
                        targetJoint = body.Joints[joint.KinectJoint.Type];

                    Transform jointObject = bodyObject.transform.FindChild(jt.ToString());
                    Renderer jointRenderer = jointObject.GetComponent<Renderer>();

                    jointObject.localPosition = Calculations.GetVector3FromJoint(sourceJoint);

                    LineRenderer lr = jointObject.GetComponent<LineRenderer>();

                    if (joint.KinectJoint.ActiveInExercise || (joint.KinectJoint.Parent != null && joint.KinectJoint.Parent.ActiveInExercise))
                    {
                        lr.material = activeInExerciseMaterial;
                    }
                    else
                    {
                        lr.material = defaultBoneMaterial;

                        if (joint.Active == false)
                            jointRenderer.material = disabledJointMaterial;
                        else if (joint.KinectJoint.Stressed)
                            jointRenderer.material = stressedJointMaterial;
                        else
                            jointRenderer.material = jointMaterial;
                    }

                    if (joint.KinectJoint.ActiveInExercise)
                        jointRenderer.material = activeInExerciseMaterial;

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