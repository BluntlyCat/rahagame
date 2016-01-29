namespace HSA.RehaGame.Kinect
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Linq;
    using System.Collections.Generic;
    using Kinect = Windows.Kinect;
    using HSA.RehaGame.Logging;
    using Scene;
    using Math;
    using DB;
    using User;
    using InGame;

    public class BodySourceView : MonoBehaviour
    {
        private static Logger<BodySourceView> logger = new Logger<BodySourceView>();

        public Material BoneMaterial;
        public GameObject BodySourceManager;

        private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
        private BodySourceManager _BodyManager;

        private Patient patient;

        private Dictionary<string, Kinect.Joint> GetJoints(Kinect.Body body, KinectJoint joint)
        {
            return new Dictionary<string, Windows.Kinect.Joint>
            {
                { "base", body.Joints[joint.JointType] },
                { "parent", body.Joints[joint.Parent.JointType] },
                { "child", body.Joints[joint.Children.First().Value.JointType] },
            };
        }

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            patient = GameState.ActivePatient == null ? new Patient("Hans").Select() as Patient : GameState.ActivePatient;
        }

        void Update()
        {
            if (Pause.Paused != true)
            {
                if (BodySourceManager == null)
                    return;

                _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
                if (_BodyManager == null)
                    return;

                Kinect.Body[] data = _BodyManager.GetData();
                if (data == null)
                    return;

                List<ulong> trackedIds = new List<ulong>();
                foreach (var body in data)
                {
                    if (body == null)
                        continue;

                    if (body.IsTracked)
                        trackedIds.Add(body.TrackingId);
                }

                List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

                // First delete untracked bodies
                foreach (ulong trackingId in knownIds)
                {
                    if (!trackedIds.Contains(trackingId))
                    {
                        Destroy(_Bodies[trackingId]);
                        _Bodies.Remove(trackingId);
                    }
                }

                foreach (var body in data)
                {
                    if (body == null)
                        continue;

                    if (body.IsTracked)
                    {
                        if (!_Bodies.ContainsKey(body.TrackingId))
                        {
                            _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                        }

                        RefreshBodyObject(body, _Bodies[body.TrackingId]);
                    }
                }
            }
        }

        private GameObject CreateBodyObject(ulong id)
        {
            GameObject body = new GameObject("Body:" + id);

            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

                LineRenderer lr = jointObj.AddComponent<LineRenderer>();
                lr.SetVertexCount(2);
                lr.material = BoneMaterial;
                lr.SetWidth(0.05f, 0.05f);

                jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                jointObj.name = jt.ToString();
                jointObj.transform.parent = body.transform;
            }

            return body;
        }

        private void DrawAngle(Kinect.Joint joint, float angle, float x, float y, float z)
        {
            var text = GameObject.FindGameObjectWithTag("Angle");

            text.transform.position = new Vector3(x, y, 0);
            text.GetComponent<Text>().text = angle.ToString("0") + "°" + " v(" + joint.Position.X + "/" + joint.Position.Y + "/" + joint.Position.Z + ")";
        }

        private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
        {
            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                var joint = patient.GetJoint(jt);

                if (joint.Active)
                {
                    Kinect.Joint sourceJoint = body.Joints[jt];
                    Kinect.Joint? targetJoint = null;

                    if (joint.Parent != null)
                        targetJoint = body.Joints[joint.Parent.JointType];

                    Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
                    jointObj.localPosition = Calculations.GetVector3FromJoint(sourceJoint);

                    LineRenderer lr = jointObj.GetComponent<LineRenderer>();
                    if (targetJoint.HasValue)
                    {
                        if (jt == Kinect.JointType.ElbowRight)
                        {
                            var joints = GetJoints(body, joint);
                            DrawAngle(body.Joints[jt], Calculations.GetAngle(joints), jointObj.localPosition.x, jointObj.localPosition.y, jointObj.localPosition.z);
                        }

                        lr.SetPosition(0, jointObj.localPosition);
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