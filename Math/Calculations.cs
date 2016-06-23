namespace HSA.RehaGame.Math
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using Kinect = Windows.Kinect;

    class Calculations
    {
        public static double GetAngle(Kinect.Joint baseJoint, Kinect.Joint parentJoint, Kinect.Joint childJoint)
        {
            var pDelta = Substract(baseJoint, parentJoint);
            var cDelta = Substract(baseJoint, childJoint);

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

        public static Vector3 GetDistance(Kinect.Joint jointX, Kinect.Joint jointY)
        {
            var xDistance = Math.Abs(jointX.Position.X - jointY.Position.X) * 100;
            var yDistance = Math.Abs(jointX.Position.Y - jointY.Position.Y) * 100;
            var zDistance = Math.Abs(jointX.Position.Z - jointY.Position.Z) * 100;

            return new Vector3(xDistance, yDistance, zDistance);
        }

        public static Vector3 Substract(Vector3 b, Vector3 t)
        {
            return new Vector3(b.x - t.x, b.y - t.y, b.z - t.z);
        }

        public static Vector3 Add(Vector3 b, Vector3 t)
        {
            return new Vector3(b.x + t.x, b.y + t.y, b.z + t.z);
        }

        public static Vector3 Substract(Kinect.Joint joint1, Kinect.Joint joint2)
        {
            var u = GetVector3FromJoint(joint1);
            var v = GetVector3FromJoint(joint2);

            return Substract(u, v);
        }

        public static Vector3 Add(Kinect.Joint joint1, Kinect.Joint joint2)
        {
            var u = GetVector3FromJoint(joint1);
            var v = GetVector3FromJoint(joint2);

            return Add(u, v);
        }

        public static float CalculateVectorScalar(Vector3 u, Vector3 v)
        {
            return u.x * v.x + u.y * v.y + u.z * v.z;
        }

        public static float CalculateVectorLength(Vector3 v)
        {
            return Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
        }

        public static double CalculateVectorLength2d(Vector3 v)
        {
            return Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
        }
    }
}
