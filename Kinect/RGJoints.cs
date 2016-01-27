namespace HSA.RehaGame.Kinect
{
    using System.Collections.Generic;
    using System.Linq;
    using DB;
    using User;
    using Kinect = Windows.Kinect;

    public class RGJoints
    {
        private static RGJoints rgJoints;
        private Dictionary<Kinect.JointType, PatientJoint> jointMap = new Dictionary<Kinect.JointType, PatientJoint>();

        private RGJoints()
        {
            var joints = DBManager.Query("SELECT * FROM editor_joint");

            CreateJoints(joints);
            LinkChildren();
        }

        private RGJoints(IList<Dictionary<string, object>> joints)
        {
            CreateJoints(joints);
            LinkChildren();
        }

        private void CreateJoints(IList<Dictionary<string, object>> joints)
        {
            foreach (var joint in joints)
            {
                var jtChild = (Kinect.JointType)System.Enum.Parse(typeof(Kinect.JointType), joint["name"].ToString());
                var parentName = joint["parent_id"].ToString();

                var xAxis = joint["x_axis"].ToString().ToLower();
                var yAxis = joint["y_axis"].ToString().ToLower();
                var zAxis = joint["z_axis"].ToString().ToLower();

                var xAxisMinValue = joint["x_axis_min_value"].ToString();
                var xAxisMaxValue = joint["x_axis_max_value"].ToString();

                var yAxisMinValue = joint["y_axis_min_value"].ToString();
                var yAxisMaxValue = joint["y_axis_max_value"].ToString();

                var zAxisMinValue = joint["z_axis_min_value"].ToString();
                var zAxisMaxValue = joint["z_axis_max_value"].ToString();

                if (jointMap.ContainsKey(jtChild) == false)
                {
                    var rgJoint = new PatientJoint(jtChild, xAxis, yAxis, zAxis, xAxisMinValue, xAxisMaxValue, yAxisMinValue, yAxisMaxValue, zAxisMinValue, zAxisMaxValue);
                    jointMap.Add(jtChild, rgJoint);
                }

                if (parentName != "")
                {
                    var jtParent = (Kinect.JointType)System.Enum.Parse(typeof(Kinect.JointType), parentName);

                    if (jointMap.ContainsKey(jtParent) == false)
                    {
                        var parentJoint = DBManager.Query("SELECT * FROM editor_joint WHERE name = '" + parentName + "';").First();

                        var xAxisParent = parentJoint["x_axis"].ToString().ToLower();
                        var yAxisParent = parentJoint["y_axis"].ToString().ToLower();
                        var zAxisParent = parentJoint["z_axis"].ToString().ToLower();

                        var xAxisMinValueParent = parentJoint["x_axis_min_value"].ToString();
                        var xAxisMaxValueParent = parentJoint["x_axis_max_value"].ToString();

                        var yAxisMinValueParent = parentJoint["y_axis_min_value"].ToString();
                        var yAxisMaxValueParent = parentJoint["y_axis_max_value"].ToString();

                        var zAxisMinValueParent = parentJoint["z_axis_min_value"].ToString();
                        var zAxisMaxValueParent = parentJoint["z_axis_max_value"].ToString();

                        var rgJointParent = new PatientJoint(jtParent,
                            xAxisParent,
                            yAxisParent,
                            zAxisParent,
                            xAxisMinValueParent,
                            xAxisMaxValueParent,
                            yAxisMinValueParent,
                            yAxisMaxValueParent,
                            zAxisMinValueParent,
                            zAxisMaxValueParent);

                        jointMap.Add(jtParent, rgJointParent);
                    }

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

        public static IDictionary<Kinect.JointType, PatientJoint> All()
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            return rgJoints.jointMap;
        }

        public static IDictionary<Kinect.JointType, PatientJoint> Copy()
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            return new Dictionary<Kinect.JointType, PatientJoint>(rgJoints.jointMap);
        }

        public static IDictionary<Kinect.JointType, PatientJoint> Copy(IList<Dictionary<string, object>> patientJointIDs)
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            var joints = new Dictionary<Kinect.JointType, PatientJoint>(rgJoints.jointMap);

            foreach(var patientJointId in patientJointIDs)
            {
                var patientJoint = DBManager.Query("SELECT * FROM editor_patientjoint WHERE id = '" + patientJointId["patientjoint_id"] + "';").First();
                var kinectJoint = DBManager.Query("SELECT * FROM editor_joint WHERE name = '" + patientJoint["kinectJoint_id"] + "';").First();
                var type = (Kinect.JointType)System.Enum.Parse(typeof(Kinect.JointType), patientJoint["kinectJoint_id"].ToString());

                joints[type].XAxis = kinectJoint["x_axis"].ToString().ToLower() == "true";
                joints[type].YAxis = kinectJoint["y_axis"].ToString().ToLower() == "true";
                joints[type].ZAxis = kinectJoint["z_axis"].ToString().ToLower() == "true";

                joints[type].XAxisMinValue = int.Parse(kinectJoint["x_axis_min_value"].ToString());
                joints[type].XAxisMaxValue = int.Parse(kinectJoint["x_axis_max_value"].ToString());

                joints[type].YAxisMinValue = int.Parse(kinectJoint["y_axis_min_value"].ToString());
                joints[type].YAxisMaxValue = int.Parse(kinectJoint["y_axis_max_value"].ToString());

                joints[type].ZAxisMinValue = int.Parse(kinectJoint["z_axis_min_value"].ToString());
                joints[type].ZAxisMaxValue = int.Parse(kinectJoint["z_axis_max_value"].ToString());

                joints[type].Active = patientJoint["active"].ToString().ToLower() == "true";

                joints[type].XAxisPatientMinValue = int.Parse(patientJoint["x_axis_min_value"].ToString());
                joints[type].XAxisPatientMaxValue = int.Parse(patientJoint["x_axis_max_value"].ToString());

                joints[type].YAxisPatientMinValue = int.Parse(patientJoint["y_axis_min_value"].ToString());
                joints[type].YAxisPatientMaxValue = int.Parse(patientJoint["y_axis_max_value"].ToString());

                joints[type].ZAxisPatientMinValue = int.Parse(patientJoint["z_axis_min_value"].ToString());
                joints[type].ZAxisPatientMaxValue = int.Parse(patientJoint["z_axis_max_value"].ToString());
            }

            return joints;
        }

        public static PatientJoint Get(Kinect.JointType type)
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            if (rgJoints.jointMap.ContainsKey(type))
                return rgJoints.jointMap[type];

            return null;
        }

        public static IList<PatientJoint> Get(params Kinect.JointType[] types)
        {
            if (rgJoints == null)
                rgJoints = new RGJoints();

            var joints = new List<PatientJoint>();

            foreach (var type in types)
                if (rgJoints.jointMap.ContainsKey(type))
                    joints.Add(rgJoints.jointMap[type]);
            
            return joints;
        }
    }
}
