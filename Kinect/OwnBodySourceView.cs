namespace HSA.RehaGame.Kinect
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using Kinect = Windows.Kinect;
    using HSA.RehaGame.Logging;
    using Scene;
    using Math;

    public class OwnBodySourceView : MonoBehaviour
    {
        private static Logger<BodySourceView> logger = new Logger<BodySourceView>();

        public Material BoneMaterial;
        public GameObject BodySourceManager;

        private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
        private BodySourceManager _BodyManager;

        private GameObject masterBone;
        private Dictionary<Kinect.JointType, GameObject> boneMap = new Dictionary<Kinect.JointType, GameObject>();

        private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
        {
            { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
            { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
            { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
            { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

            { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
            { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
            { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
            { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

            { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
            { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
            { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
            { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
            { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
            { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

            { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
            { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
            { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
            { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
            { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
            { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

            { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
            { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
            { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
            { Kinect.JointType.Neck, Kinect.JointType.Head },
        };

        private GameObject GetBone(GameObject parent, string name)
        {
            if (parent.name == name)
                return parent;

            foreach(Transform child in parent.transform)
            {
                var bone = GetBone(child.gameObject, name);

                if (bone)
                    return bone;
            }

            return null;
        }

        void Start()
        {
            logger.AddLogAppender<ConsoleAppender>();
            masterBone = GameObject.FindGameObjectWithTag("MasterBone");

            if (masterBone)
            {
                for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
                {
                    var bone = GetBone(masterBone, jt.ToString());

                    if(bone)
                        boneMap.Add(jt, bone);
                }
            }
        }

        void Update()
        {
            if (Pause.Paused != true)
            {
                if (BodySourceManager == null)
                {
                    return;
                }

                _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
                if (_BodyManager == null)
                {
                    return;
                }

                Kinect.Body[] data = _BodyManager.GetData();
                if (data == null)
                {
                    return;
                }

                List<ulong> trackedIds = new List<ulong>();
                foreach (var body in data)
                {
                    if (body == null)
                    {
                        continue;
                    }

                    if (body.IsTracked)
                    {
                        trackedIds.Add(body.TrackingId);
                    }
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
                    {
                        continue;
                    }

                    if (body.IsTracked)
                    {
                        RefreshBodyObject(body, _Bodies[body.TrackingId]);
                    }
                }
            }
        }

        private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
        {
            for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
            {
                Kinect.Joint sourceJoint = body.Joints[jt];
                Kinect.Joint? targetJoint = null;

                if (_BoneMap.ContainsKey(jt))
                {
                    targetJoint = body.Joints[_BoneMap[jt]];

                    Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
                    jointObj.localPosition = Calculations.GetVector3FromJoint(sourceJoint);
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