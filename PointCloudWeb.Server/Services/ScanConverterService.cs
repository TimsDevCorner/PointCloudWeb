using System;
using System.Numerics;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class ScanConverterService
    {
        private enum RotationAxis { X, Y, Z };

        private Matrix4x4 GetTransformationMatrix(ScanDataPoint scan, RotationAxis type)
        {
            var angle = type switch
            {
                RotationAxis.X => scan.RAX,
                RotationAxis.Y => scan.RAY,
                RotationAxis.Z => scan.RAZ,
                _ => throw new NotImplementedException(),
            };

            if (angle == 0)
                return new Matrix4x4(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                    );

            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);

            return type switch
            {
                RotationAxis.X => new Matrix4x4(
                                       1, 0, 0, 0,
                                       0, sin, -sin, 0,
                                       0, sin, cos, 0,
                                       0, 0, 0, 1
                                       ),
                RotationAxis.Y => new Matrix4x4(
                                      cos, 0, sin, 0,
                                      0, 1, 0, 0,
                                      -sin, 0, cos, 0,
                                      0, 0, 0, 1
                                      ),
                RotationAxis.Z => new Matrix4x4(
                                      cos, -sin, 0, 0,
                                      sin, cos, 0, 0,
                                      0, 0, 1, 0,
                                      0, 0, 0, 1
                                      ),
                _ => throw new NotImplementedException(),
            };
        }

        public Point Transform(ScanDataPoint scan)
        {
            Vector3 v = new Vector3(0, 0, (int)scan.DistanceMM);
            Vector3.Transform(v, GetTransformationMatrix(scan, RotationAxis.X));
            Vector3.Transform(v, GetTransformationMatrix(scan, RotationAxis.Z));
            Vector3.Transform(v, GetTransformationMatrix(scan, RotationAxis.Y));

            return new Point(v.X, v.Y, v.Z);
        }
    }
}