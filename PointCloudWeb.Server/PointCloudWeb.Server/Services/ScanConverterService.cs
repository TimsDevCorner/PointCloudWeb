using System;
using System.Numerics;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class ScanConverterService
    {
        private int Round(double value) => (int)Math.Round(double.IsNaN(value) ? 0 : value, 0, MidpointRounding.AwayFromZero);
        public Point Transform(ScanDataPoint scan)
        {
            var factorZ = 1;

            var degreeXA = scan.RAX;
            var degreeYA = scan.RAY;

            if (degreeXA > 270 && degreeYA > 270)
            {
                degreeXA -= 270;
                degreeYA -= 270;
                factorZ = -1;
            }

            var degreeXB = 180 - 90 - degreeXA;
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
                Z = factorZ * Round(z)
            };

            return p;
        }
    }
}