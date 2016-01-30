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
            var p = GetVector3FromJoint(joints["parent"]);
            var c = GetVector3FromJoint(joints["child"]);

            var pDelta = Substract(b, p);
            var cDelta = Substract(b, c);

            var scalar = CalculateVectorScalar(pDelta, cDelta);
            var length_u = CalculateVectorLength(pDelta);
            var length_v = CalculateVectorLength(cDelta);
            var cos_phi = scalar / (length_u * length_v);

            return (Mathf.Acos(cos_phi) * 180) / Mathf.PI;
        }

        public static Vector3 GetVector3FromJoint(Kinect.Joint joint, bool x = true, bool y = true, bool z = true)
        {
            float X = x ? joint.Position.X : 0;
            float Y = y ? joint.Position.Y : 0;
            float Z = z ? joint.Position.Z : 0;

            return new Vector3(X * 10, Y * 10, Z * 10);
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
