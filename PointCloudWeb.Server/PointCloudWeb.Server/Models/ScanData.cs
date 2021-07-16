using System;
using System.Collections.Generic;

namespace PointCloudWeb.Server.Models
{
    public class ScanDataList
    {
        public Guid Id { get; set; }
        public IList<ScanDataPoint> ScanPoints { get; set; }
    }

    public class ScanDataPoint
    {
        public ScanDataPoint()
        {
            RAY = 0;
            RAX = 0;
            DistanceMM = 0;
        }

        public ScanDataPoint(double rax, double ray, float distanceMM) : this()
        {
            RAX = rax;
            RAY = ray;
            DistanceMM = distanceMM;
        }

        public float DistanceMM { get; set; }

        //RotationAngle on {X, Y} Axis
        public double RAX { get; set; }

        public double RAY { get; set; }
    }
}