namespace HSA.RehaGame.User.Kinect
{
    using System.Collections.Generic;
    using DB;
    using DB.Entities;
    using User;
    using Windows.Kinect;

    public class KinectJointManager
    {
        private Dictionary<JointType, PatientJoint> joints = new Dictionary<JointType, PatientJoint>();
        private Database database;

        public KinectJointManager(Database database, string patientName)
        {
            var table = database.Select("editor_joint");

            this.database = database;
            CreateJoints(patientName, table);
            LinkChildren();
        }

        private void CreateJoints(string patientName, Table table)
        {
            foreach (var row in table)
            {
                var jtChild = (JointType)System.Enum.Parse(typeof(JointType), row.Column("name").GetValue<string>());
                var parentName = row.Column("parent_id").GetValue<string>();

                var translation = row.Column("translation").GetValue<string>();

                var xAxis = row.Column("x_axis").GetValue<bool>();
                var yAxis = row.Column("y_axis").GetValue<bool>();
                var zAxis = row.Column("z_axis").GetValue<bool>();

                var xAxisMinValue = row.Column("x_axis_min_value").GetValue<int>();
                var xAxisMaxValue = row.Column("x_axis_max_value").GetValue<int>();

                var yAxisMinValue = row.Column("y_axis_min_value").GetValue<int>();
                var yAxisMaxValue = row.Column("y_axis_max_value").GetValue<int>();

                var zAxisMinValue = row.Column("z_axis_min_value").GetValue<int>();
                var zAxisMaxValue = row.Column("z_axis_max_value").GetValue<int>();

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
                        zAxisMaxValue,
                        database
                    );

                    joints.Add(jtChild, patientJoint);
                }

                if (parentName != "")
                {
                    var jtParent = (JointType)System.Enum.Parse(typeof(JointType), parentName);

                    if (joints.ContainsKey(jtParent) == false)
                    {
                        var parentJoint = database.SelectWhere("editor_joint", "name", parentName);

                        var translationParent = parentJoint.Column("translation").GetValue<string>();

                        var xAxisParent = parentJoint.Column("x_axis").GetValue<bool>();
                        var yAxisParent = parentJoint.Column("y_axis").GetValue<bool>();
                        var zAxisParent = parentJoint.Column("z_axis").GetValue<bool>();

                        var xAxisMinValueParent = parentJoint.Column("x_axis_min_value").GetValue<int>();
                        var xAxisMaxValueParent = parentJoint.Column("x_axis_max_value").GetValue<int>();

                        var yAxisMinValueParent = parentJoint.Column("y_axis_min_value").GetValue<int>();
                        var yAxisMaxValueParent = parentJoint.Column("y_axis_max_value").GetValue<int>();

                        var zAxisMinValueParent = parentJoint.Column("z_axis_min_value").GetValue<int>();
                        var zAxisMaxValueParent = parentJoint.Column("z_axis_max_value").GetValue<int>();

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
                            zAxisMaxValueParent,
                            database);

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
                //var children = database.Query("editor_joint_children", "SELECT to_joint_id FROM editor_joint_children WHERE from_joint_id = '" + jt.Key + "'");
                var children = database.Join(jt.Key, "editor_joint_children", "from_joint_id", "to_joint_id", "editor_joint");

                Dictionary<JointType, PatientJoint> dict = new Dictionary<JointType, PatientJoint>();

                foreach (var child in children)
                {
                    var jtChild = (JointType)System.Enum.Parse(typeof(JointType), child.Column("name").GetValue<string>());
                    dict.Add(jtChild, joints[jtChild]);
                }

                jt.Value.Children = dict;
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
