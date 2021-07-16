using System;
using System.Numerics;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class ScanConverterService
    {

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

        private int Round(double value) => (int)Math.Round(value, 0, MidpointRounding.AwayFromZero);
        public Point Transform(ScanDataPoint scan)
        {
            var degreeXA = scan.RAX;
            var degreeXB = 180 - 90 - degreeXA;
            var degreeYA = scan.RAY;
            var degreeYB = 180 - 90 - degreeYA;

            var radXA = degreeXA * Math.PI / 180;
            var radXB = degreeXB * Math.PI / 180;
            var radYA = degreeYA * Math.PI / 180;
            var radYB = degreeYB * Math.PI / 180;

            double sinXA = Math.Sin(radXA);
            double sinXB = Math.Sin(radXB);
            double sinYA = Math.Sin(radYA);
            double sinYB = Math.Sin(radYB);

            var z = Math.Sqrt(
                Math.Pow(
                    Math.Pow(sinXB, 2) / Math.Pow(sinXA, 2)
                    + Math.Pow(sinYB, 2) / Math.Pow(sinYA, 2)
                    + 1
                    , -1)
                * Math.Pow(scan.DistanceMM, 2)
                );


            var p = new Point()
            {
                X = Round(z * sinYB / sinYA),
                Y = Round(z * sinXB / sinXA),
                Z = Round(z)
            };

            return p;
        }
    }
}