using System;
using System.Numerics;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class ScanConverterService
    {
        private enum RotationAxis { X, Y, Z };

        private static void CorrectQuadrants(double angle, ref float sin, ref float cos)
        {
            if (angle < 0 || angle >= 360)
                throw new ArgumentOutOfRangeException();
            if (angle > 90 && angle < 180)
            {
                cos *= -1;
            }
            else if (angle >= 180 && angle < 270)
            {
                sin *= -1;
                cos *= -1;
            }
            else if (angle >= 270 && angle < 360)
            {
                sin *= -1;
            }
        }

        private static Matrix4x4 GetTransformationMatrix(ScanDataPoint scan, RotationAxis type)
        {
            double angle = type switch
            {
                RotationAxis.X => scan.RAX,//360 - 90 + scan.RAX,
                RotationAxis.Y => -scan.RAY,//360 - scan.RAY,
                RotationAxis.Z => 0,//360 - scan.RAY,
                _ => throw new NotImplementedException(),
            };

            if (angle == 0)
                return new Matrix4x4(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                    );

            angle %= 360;
            if (angle < 0)
                angle += 360;

            var angleInR = angle * (Math.PI / 180.0);

            var sin = (float)Math.Sin(angleInR);
            var cos = (float)Math.Cos(angleInR);

            //CorrectQuadrants(angle, ref sin, ref cos);

            return type switch
            {
                RotationAxis.X => new Matrix4x4(
                                       1, 0, 0, 0,
                                       0, cos, -sin, 0,
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
            Vector3 v = new Vector3(0, 0, 0);

            v.Z = scan.DistanceMM;

            var matrixX = GetTransformationMatrix(scan, RotationAxis.X);
            var matrixY = GetTransformationMatrix(scan, RotationAxis.Y);
            var matrixZ = GetTransformationMatrix(scan, RotationAxis.Z);

            v = Vector3.Transform(v, matrixX);
            v = Vector3.Transform(v, matrixY);
            v = Vector3.Transform(v, matrixZ);

            return new Point(
                (int)Math.Round(v.X, 0, MidpointRounding.AwayFromZero),
                (int)Math.Round(v.Y, 0, MidpointRounding.AwayFromZero),
                (int)Math.Round(v.Z, 0, MidpointRounding.AwayFromZero)
                );
        }
    }
}