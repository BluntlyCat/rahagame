namespace HSA.RehaGame.Kinect
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Linq;
    using System.Collections.Generic;
    using Kinect = Windows.Kinect;
    using Logging;
    using Scene;
    using Math;
    using InGame;
    using Exercises;
    using User;

    public class BodySourceView : MonoBehaviour
    {
        private static Logger<BodySourceView> logger = new Logger<BodySourceView>();

        public Material boneMaterial;
        public Material jointMaterial;
        public Material stressedJointMaterial;
        public Material disabledJointMaterial;

        public Transform jointInfoText;

        public GameObject bodySourceManager;
        private Dictionary<ulong, GameObject> kinectBodies = new Dictionary<ulong, GameObject>();
        private BodySourceManager bodyManager;
        private List<ulong> trackedIds;

        private Dictionary<Kinect.JointType, Text> jointInfo = new Dictionary<Kinect.JointType, Text>();

        private Exercise exercise;

        private Dictionary<string, Kinect.Joint> GetJoints(Kinect.Body body, KinectJoint joint)
        {
            return new Dictionary<string, Kinect.Joint>
            {
                { "base", body.Joints[joint.JointType] },
                { "parent", body.Joints[joint.Parent.JointType] },
                { "child", body.Joints[joint.Children.First().Value.JointType] },
            };
        }

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            bodyManager = bodySourceManager.GetComponent<BodySourceManager>();
            exercise = new Exercise("exercise1", new Patient("michael").Select() as Patient).Select() as Exercise;
        }

        private Kinect.Body[] GetBodies()
        {
            var bodies = bodyManager.GetData();

            if (bodies == null)
                return null;

            trackedIds = new List<ulong>();
            foreach (var body in bodies)
            {
                if (body == null)
                    continue;

                if (body.IsTracked)
                    trackedIds.Add(body.TrackingId);
            }

            return bodies;
        }

        private void DeleteUntrackedBodies()
        {
            List<ulong> knownIds = new List<ulong>(kinectBodies.Keys);

            // First delete untracked bodies
            foreach (ulong trackingId in knownIds)
            {
                if (!trackedIds.Contains(trackingId))
                {
                    Destroy(kinectBodies[trackingId]);
                    kinectBodies.Remove(trackingId);
                }
            }
        }

        void Update()
        {
            if (Pause.Paused)
                return;

            Kinect.Body[] bodies = GetBodies();

            if (bodies == null)
                return;

            DeleteUntrackedBodies();

            foreach (var body in bodies)
            {
                if (body == null)
                    continue;

                if (body.IsTracked)
                {
                    if (!kinectBodies.ContainsKey(body.TrackingId))
                    {
                        kinectBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    }

                    RefreshBodyObject(body, kinectBodies[body.TrackingId]);
                }

                exercise.DoStep(body);
            }
        }

        private GameObject CreateBodyObject(ulong id)
        {
            GameObject body = new GameObject("Body:" + id);

            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                PatientJoint patientJoint = exercise.Patient.GetJoint(jt);
                LineRenderer lr = jointObj.AddComponent<LineRenderer>();
                Renderer jointRenderer = jointObj.GetComponent<Renderer>();

                if(patientJoint.Active == false)
                    jointRenderer.material = disabledJointMaterial;
                else if(patientJoint.Stressed)
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

        private void DrawAngle(Kinect.Joint joint, float angle)
        {
            var text = jointInfo[joint.JointType];

            text.transform.position = Calculations.GetVector3FromJoint(joint);
            text.text = string.Format("Angle: {0}°\nX: {1}\nY: {2}\nZ: {3}", angle.ToString("0"), joint.Position.X.ToString("0.000"), joint.Position.Y.ToString("0.000"), joint.Position.Z.ToString("0.000"));
        }

        private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
        {
            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                var joint = exercise.Patient.GetJoint(jt);

                Kinect.Joint sourceJoint = body.Joints[jt];
                Kinect.Joint? targetJoint = null;

                if (joint.Parent != null)
                    targetJoint = body.Joints[joint.Parent.JointType];

                Transform jointObject = bodyObject.transform.FindChild(jt.ToString());
                jointObject.localPosition = Calculations.GetVector3FromJoint(sourceJoint);

                LineRenderer lr = jointObject.GetComponent<LineRenderer>();

                if (targetJoint.HasValue)
                {
                    if (jt == exercise.Step.Behaviour.ActiveJoint.JointType)
                    {
                        if (jointInfo.ContainsKey(jt) == false)
                        {
                            var infoText = Instantiate(jointInfoText).GetComponent<Text>();
                            infoText.transform.SetParent(GameObject.Find("JointInfoCanvas").transform, false);

                            jointInfo.Add(jt, infoText);
                        }

                        var joints = GetJoints(body, joint);
                        DrawAngle(body.Joints[jt], Calculations.GetAngle(joints));
                    }

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