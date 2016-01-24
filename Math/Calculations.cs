namespace HSA.RehaGame.Math
{
    using System.Collections.Generic;
    using UnityEngine;
    using Kinect = Windows.Kinect;

    class Calculations
    {
        public static float GetAngle(Dictionary<string, Kinect.Joint> joints)
        {
            var b = GetVector3FromJoint(joints["base"]);
            var u = GetVector3FromJoint(joints["parent"]);
            var v = GetVector3FromJoint(joints["child"]);

            u = Substract(b, u);
            v = Substract(b, v);

            var scalar = CalculateVectorScalar(u, v);
            var length_u = CalculateVectorLength(u);
            var length_v = CalculateVectorLength(v);
            var cos_phi = scalar / (length_u * length_v);

            return (Mathf.Acos(cos_phi) * 180) / Mathf.PI;
        }

        public static Vector3 GetVector3FromJoint(Kinect.Joint joint)
        {
            return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
        }

        public static Vector3 Substract(Vector3 b, Vector3 t)
        {
            return new Vector3(b.x - t.x, b.y - t.y, b.z - t.z);
        }

        public static float CalculateVectorScalar(Vector3 u, Vector3 v)
        {
            return u.x * v.x + u.y * v.y + u.z * v.z;
        }

        public static float CalculateVectorLength(Vector3 v)
        {
            return Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
        }
    }
}
