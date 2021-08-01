using System;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Utils;

namespace PointCloudWeb.Server.Services
{
    public class ScanConverterService
    {
        public Point Transform(ScanDataPoint scan)
        {
            if (scan.RAX >= 180 || scan.RAY >= 180)
                return new Point(0, 0, 0);

            var degreeXa = scan.RAX;
            var degreeYa = scan.RAY;

            //if (degreeXA > 270 && degreeYA > 270)
            //{
            //    degreeXA -= 270;
            //    degreeYA -= 270;
            //    factorZ = -1;
            //}

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
                Y = NumericUtils.Round(z * sinXb / sinXa),
                Z = NumericUtils.Round(z)
            };

            return p;
        }
    }
}