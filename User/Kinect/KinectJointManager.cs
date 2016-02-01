namespace HSA.RehaGame.User.Kinect
{
    using System.Collections.Generic;
    using DB;
    using User;
    using Windows.Kinect;

    public class KinectJointManager
    {
        private Dictionary<JointType, PatientJoint> joints = new Dictionary<JointType, PatientJoint>();

        public KinectJointManager(string patientName)
        {
            var table = DBManager.Query("editor_joint", "SELECT * FROM editor_joint");

            CreateJoints(patientName, table);
            LinkChildren();
        }

        private void CreateJoints(string patientName, DBTable table)
        {
            foreach (var row in table.Rows)
            {
                var jtChild = (JointType)System.Enum.Parse(typeof(JointType), row.GetValue("name"));
                var parentName = row.GetValue("parent_id");

                var translation = row.GetValueFromLanguage("translation");

                var xAxis = row.GetBool("x_axis");
                var yAxis = row.GetBool("y_axis");
                var zAxis = row.GetBool("z_axis");

                var xAxisMinValue = row.GetInt("x_axis_min_value");
                var xAxisMaxValue = row.GetInt("x_axis_max_value");

                var yAxisMinValue = row.GetInt("y_axis_min_value");
                var yAxisMaxValue = row.GetInt("y_axis_max_value");

                var zAxisMinValue = row.GetInt("z_axis_min_value");
                var zAxisMaxValue = row.GetInt("z_axis_max_value");

                if (joints.ContainsKey(jtChild) == false)
                {
                    var patientJoint = new PatientJoint(patientName,
                        jtChild,
                        translation,
                        xAxis,
                        yAxis,
                        zAxis,
                        xAxisMinValue,
                        xAxisMaxValue,
                        yAxisMinValue,
                        yAxisMaxValue,
                        zAxisMinValue,
                        zAxisMaxValue
                    );

                    joints.Add(jtChild, patientJoint);
                }

                if (parentName != "")
                {
                    var jtParent = (JointType)System.Enum.Parse(typeof(JointType), parentName);

                    if (joints.ContainsKey(jtParent) == false)
                    {
                        var parentJoint = DBManager.Query("editor_joint", "SELECT * FROM editor_joint WHERE name = '" + parentName + "';").GetRow();

                        var translationParent = parentJoint.GetValueFromLanguage("translation");

                        var xAxisParent = parentJoint.GetBool("x_axis");
                        var yAxisParent = parentJoint.GetBool("y_axis");
                        var zAxisParent = parentJoint.GetBool("z_axis");

                        var xAxisMinValueParent = parentJoint.GetInt("x_axis_min_value");
                        var xAxisMaxValueParent = parentJoint.GetInt("x_axis_max_value");

                        var yAxisMinValueParent = parentJoint.GetInt("y_axis_min_value");
                        var yAxisMaxValueParent = parentJoint.GetInt("y_axis_max_value");

                        var zAxisMinValueParent = parentJoint.GetInt("z_axis_min_value");
                        var zAxisMaxValueParent = parentJoint.GetInt("z_axis_max_value");

                        var patientJointParent = new PatientJoint(patientName,
                            jtParent,
                            translationParent,
                            xAxisParent,
                            yAxisParent,
                            zAxisParent,
                            xAxisMinValueParent,
                            xAxisMaxValueParent,
                            yAxisMinValueParent,
                            yAxisMaxValueParent,
                            zAxisMinValueParent,
                            zAxisMaxValueParent);

                        joints.Add(jtParent, patientJointParent);
                    }

                    joints[jtChild].Parent = joints[jtParent];
                }
            }
        }

        private void LinkChildren()
        {
            foreach (var jt in joints)
            {
                var children = DBManager.Query("editor_joint_children", "SELECT to_joint_id FROM editor_joint_children WHERE from_joint_id = '" + jt.Key + "'");

                if (children.Rows.Count > 0)
                {
                    Dictionary<JointType, PatientJoint> dict = new Dictionary<JointType, PatientJoint>();

                    foreach (var child in children.Rows)
                    {
                        var jtChild = (JointType)System.Enum.Parse(typeof(JointType), child.GetValue("to_joint_id"));
                        dict.Add(jtChild, joints[jtChild]);
                    }

                    jt.Value.Children = dict;
                }
            }
        }

        public Dictionary<JointType, PatientJoint> Joints
        {
            get
            {
                return joints;
            }
        }
    }
}
