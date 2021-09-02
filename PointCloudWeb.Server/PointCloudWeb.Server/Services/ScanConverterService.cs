using System;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class ScanConverterService
    {
        public static Point Transform(ScanDataPoint scan)
        {
            var alpha = (90 - scan.RAX) * Math.PI / 180;
            var beta = scan.RAY * Math.PI / 180;

            var p = new Point(
                z: (int)(scan.DistanceMM * Math.Sin(alpha)),
                x: (int)(scan.DistanceMM * Math.Cos(beta) * Math.Cos(alpha)),
                y: (int)(scan.DistanceMM * Math.Sin(beta) * Math.Cos(alpha))
            );
            return p;
        }
    }
}