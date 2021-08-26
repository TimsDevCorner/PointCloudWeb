using System;
using System.Numerics;

namespace PointCloudWeb.Server.Utils
{
    public class MatrixUtils
    {
        public static Vector3 RotationMatrixToAngles(Matrix4x4 m)
        {
            var result = Vector3.Zero;

            result.X = (float)Math.Atan2(m.M32, m.M33);
            result.Y = (float)Math.Atan2(-m.M31, Math.Sqrt(m.M32 * m.M32 + m.M33 * m.M33));
            result.Z = (float)Math.Atan2(m.M21, m.M11);

            return result;
        }
    }
}