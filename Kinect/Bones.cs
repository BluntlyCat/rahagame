namespace HSA.RehaGame.KinectManager
{
    using System.Collections.Generic;
    using Logging;
    using UnityEngine;
    using Windows.Kinect;

    public class Bones
    {
        private static Logger<Bones> logger = new Logger<Bones>();

        private Dictionary<string, GameObject> modelBones = new Dictionary<string, GameObject>();
        private Dictionary<JointType, string> kinectBones = new Dictionary<JointType, string>()
        {
            { JointType.Head, "Head" },
            { JointType.Neck, "Nech" },
            //{ JointType.SpineShoulder, "Neck" },

            { JointType.ShoulderLeft, "LeftArm" },
            { JointType.ShoulderRight, "RightArm" },

            { JointType.ElbowLeft, "LeftForeArm" },
            { JointType.ElbowRight, "RightForeArm" },

            { JointType.WristLeft, "LeftHand" },
            { JointType.WristRight, "RightHand" },

            { JointType.HandLeft, "LeftFingerBase" },
            { JointType.HandRight, "RightFingerBase" },

            // { JointType.HandTipLeft, "" },
            // { JointType.HandTipRight, "" },

            { JointType.ThumbLeft, "LeftHandThumb1" },
            { JointType.ThumbRight, "RightHandThumb1" },

            { JointType.SpineBase, "Hips" },
            { JointType.SpineMid, "Spine2" },

            { JointType.HipLeft, "LeftUpLeg" },
            { JointType.HipRight, "RightUpLeg" },

            { JointType.KneeLeft, "LeftLeg" },
            { JointType.KneeRight, "RightLeg" },

            { JointType.AnkleLeft, "LeftFoot" },
            { JointType.AnkleRight, "RightFoot" },

            { JointType.FootLeft, "LeftToeBase" },
            { JointType.FootRight, "RightToeBase" },
        };

        private Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
        {
            return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
        }

        private Quaternion GetQuaternionFromJointOrientation(Windows.Kinect.JointOrientation orientation)
        {
            return new Quaternion(
                orientation.Orientation.X * 10,
                orientation.Orientation.Y * 10,
                orientation.Orientation.Z * 10,
                orientation.Orientation.W * 10
            );
        }

        private void SetModelBones(GameObject parentBone)
        {
            for (int i = 0; i < parentBone.transform.childCount; i++)
            {
                GameObject childBone = parentBone.transform.GetChild(i).gameObject;

                childBone.transform.parent = parentBone.transform;
                
                if (childBone.transform.childCount > 0)
                    SetModelBones(childBone);
                else
                    modelBones.Add(childBone.name, childBone);
            }

            modelBones.Add(parentBone.name, parentBone);
        }

        public Bones()
        {
            logger.AddLogAppender<ConsoleAppender>();
            GameObject masterBone = GameObject.FindGameObjectWithTag("MasterBone");

            if (masterBone != null)
            {
                this.SetModelBones(masterBone);
            }
        }


        public void SetBonePosition(Body body, GameObject player)
        {
            foreach (var joint in body.Joints)
            {
                var key = joint.Key;
                
                if(kinectBones.ContainsKey(key))
                {
                    var kinectBone = kinectBones[joint.Key];
                    var orientation = body.JointOrientations[joint.Value.JointType];

                    if (modelBones.ContainsKey(kinectBone))
                    {
                        var modelBone = modelBones[kinectBone].transform;

                        var position = this.GetVector3FromJoint(joint.Value);
                        var rotation = this.GetQuaternionFromJointOrientation(orientation);

                        modelBone.position = position;
                        //modelBone.rotation = rotation;
                    }
                }
            }
        }
    }
}