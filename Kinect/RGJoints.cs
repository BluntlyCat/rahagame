namespace HSA.RehaGame.Kinect
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DB;
    using Kinect = Windows.Kinect;

    public class RGJoints
    {
        private static RGJoints rgJoints;
        private Dictionary<Kinect.JointType, RGJoint> jointMap = new Dictionary<Kinect.JointType, RGJoint>();

        private RGJoints()
        {
            var joints = DBManager.Query("SELECT name, parent_id FROM editor_joint");

            CreateJoints(joints);
            LinkChildren();
        }

        private void CreateJoints(IList<Dictionary<string, object>> joints)
        {
            foreach (var joint in joints)
            {
                var jtChild = (Kinect.JointType)System.Enum.Parse(typeof(Kinect.JointType), joint["name"].ToString());
                var parentName = joint["parent_id"].ToString();

                if (jointMap.ContainsKey(jtChild) == false)
                    jointMap.Add(jtChild, new RGJoint(jtChild));

                if (parentName != "")
                {
                    var jtParent = (Kinect.JointType)System.Enum.Parse(typeof(Kinect.JointType), parentName);

                    if (jointMap.ContainsKey(jtParent) == false)
                        jointMap.Add(jtParent, new RGJoint(jtParent));

                    jointMap[jtChild].Parent = jointMap[jtParent];
                }
            }
        }

        private void LinkChildren()
        {
            foreach (var jt in jointMap)
            {
                var children = DBManager.Query("SELECT to_joint_id FROM editor_joint_children WHERE from_joint_id = '" + jt.Key + "'");

                if (children.Count > 0)
                {
                    Dictionary<Kinect.JointType, RGJoint> dict = new Dictionary<Kinect.JointType, RGJoint>();

                    foreach (var child in children)
                    {
                        var jtChild = (Kinect.JointType)System.Enum.Parse(typeof(Kinect.JointType), child["to_joint_id"].ToString());
                        dict.Add(jtChild, jointMap[jtChild]);
                    }

                    jt.Value.Children = dict;
                }
            }
        }

        public static IDictionary<Kinect.JointType, RGJoint> All()
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            return rgJoints.jointMap;
        }

        public static IDictionary<Kinect.JointType, RGJoint> Copy()
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            return new Dictionary<Kinect.JointType, RGJoint>(rgJoints.jointMap);
        }

        public static RGJoint Get(Kinect.JointType type)
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            if (rgJoints.jointMap.ContainsKey(type))
                return rgJoints.jointMap[type];

            return null;
        }

        public static IList<RGJoint> Get(params Kinect.JointType[] types)
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            var joints = new List<RGJoint>();

            foreach (var type in types)
                if (rgJoints.jointMap.ContainsKey(type))
                    joints.Add(rgJoints.jointMap[type]);
            
            return joints;
        }
    }
}
