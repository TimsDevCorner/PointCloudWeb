using System;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class ScanConverterService
    {
        public static Point Transform(ScanDataPoint scan)
        {
            var alpha = scan.RAX * Math.PI / 180;
            var beta = scan.RAY * Math.PI / 180;

            var p = new Point(
                y: (int)(scan.DistanceMM * Math.Sin(alpha)),
                x: (int)(scan.DistanceMM * Math.Cos(beta) * Math.Cos(alpha)),
                z: (int)(scan.DistanceMM * Math.Sin(beta) * Math.Cos(alpha))
            );
            return p;
        }
    }
}