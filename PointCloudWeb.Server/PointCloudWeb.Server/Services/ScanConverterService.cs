using System;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Utils;

namespace PointCloudWeb.Server.Services
{
    public class ScanConverterService
    {
        public static Point Transform(ScanDataPoint scan)
        {
            // if (scan.RAX >= 90 || scan.RAY >= 90)
            //     return new Point(0, 0, 0);

            var degreeXa = scan.RAX;
            var degreeYa = scan.RAY;

            var factorY = 1;
            var factorZ = 1;
            if (180 <= degreeXa && degreeXa <= 360)
            {
                 factorY = -1;
                 factorZ = -1;
            }
                 
            var degreeXb = 180 - 90 - degreeXa;
            var degreeYb = 180 - 90 - degreeYa;

            var radXa = degreeXa * Math.PI / 180;
            var radXb = degreeXb * Math.PI / 180;
            var radYa = degreeYa * Math.PI / 180;
            var radYb = degreeYb * Math.PI / 180;

            var sinXa = Math.Sin(radXa);
            var sinXb = Math.Sin(radXb);
            var sinYa = Math.Sin(radYa);
            var sinYb = Math.Sin(radYb);

            if (sinXa == 0)
            {
                sinXa = 1;
                sinXb = 0;
            }
            if (sinYa == 0)
            {
                sinYa = 1;
                sinYb = 0;
            }

            var z = Math.Sqrt(
                Math.Pow(
                    Math.Pow(sinXb, 2) / Math.Pow(sinXa, 2)
                    + Math.Pow(sinYb, 2) / Math.Pow(sinYa, 2)
                    + 1
                    , -1)
                * Math.Pow(scan.DistanceMM, 2)
                );

            var p = new Point()
            {
                X = NumericUtils.Round(z * sinYb / sinYa),
                Y = factorY * NumericUtils.Round(z * sinXb / sinXa),
                Z = factorZ * NumericUtils.Round(z)
            };

            return p;
        }
    }
}