namespace HSA.RehaGame.Input.Kinect
{
    using System.Collections.Generic;
    using Exercises;
    using InGame;
    using Logging;
    using Math;
    using UnityEngine;
    using User;
    using Kinect = Windows.Kinect;

    public class BodyManager : MonoBehaviour
    {
        private static Logger<BodyManager> logger = new Logger<BodyManager>();

        public Material boneMaterial;
        public Material jointMaterial;
        public Material stressedJointMaterial;
        public Material disabledJointMaterial;

        public GameObject bodySourceManager;
        public GameObject informationCanvasObject;

        private Canvas informationCanvas;

        private Dictionary<ulong, GameObject> kinectBodies = new Dictionary<ulong, GameObject>();
        private BodySourceManager bodyManager;
        private List<ulong> trackedIds;

        private Exercise exercise;

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            bodyManager = bodySourceManager.GetComponent<BodySourceManager>();
            informationCanvas = informationCanvasObject.GetComponent<Canvas>();

            // ToDo Set exercise from gamestate
            //GameState.ActivePatient = GameState.ActivePatient == null ? new Patient("Michael").Select() as Patient : GameState.ActivePatient;
            //GameState.ActiveExercise = GameState.ActiveExercise == null ? new Exercise("exercise1", GameState.ActivePatient).Select() as Exercise : GameState.ActiveExercise;
            exercise = GameState.ActiveExercise;
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
            if (exercise.IsActive)
            {
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
                        exercise.DoExercise(body);

                        if (!kinectBodies.ContainsKey(body.TrackingId))
                        {
                            kinectBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                        }

                        RefreshBodyObject(body, kinectBodies[body.TrackingId]);
                        return;
                    }
                }
            }
        }

        private GameObject CreateBodyObject(ulong id)
        {
            GameObject body = new GameObject("Body:" + id);
            body.transform.SetParent(informationCanvas.transform, true);

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

        private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
        {
            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                var joint = exercise.Patient.GetJoint(jt);

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