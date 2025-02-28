using System;
using System.Linq;
using UnityEngine;


namespace UniHumanoid
{
    public static class BvhExtensions
    {
        public static Func<float, float, float, Quaternion> GetEulerToRotation(this BvhNode bvh)
        {
            var order = bvh.Channels.Where(x => x == BvhChannel.Xrotation || x == BvhChannel.Yrotation || x == BvhChannel.Zrotation).ToArray();

            return (x, y, z) =>
            {
                var xRot = Quaternion.Euler(x, 0, 0);
                var yRot = Quaternion.Euler(0, y, 0);
                var zRot = Quaternion.Euler(0, 0, z);

                var r = Quaternion.identity;
                foreach (var ch in order)
                {
                    switch (ch)
                    {
                        case BvhChannel.Xrotation: r = r * xRot; break;
                        case BvhChannel.Yrotation: r = r * yRot; break;
                        case BvhChannel.Zrotation: r = r * zRot; break;
                        default: throw new BvhException("no rotation");
                    }
                }
                return r;
            };
        }

        public static Vector3 ToVector3(this Vector3 s3)
        {
            return new Vector3(s3.x, s3.y, s3.z);
        }

        public static Vector3 ToXReversedVector3(this Vector3 s3)
        {
            return new Vector3(-s3.x, s3.y, s3.z);
        }
    }
}
