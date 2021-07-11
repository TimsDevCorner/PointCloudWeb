
using System;
using System.Collections.Generic;

namespace PointCloudWeb.Server.Models
{
    public class ScanDataPoint
    {
        //RotationAngle on {Y,Z,X} Axis
        public double RAY { get; set; }
        public double RAZ { get; set; }
        public double RAX { get; set; }
        public double DistanceMM { get; set; }
    }

    public class ScanDataList
    {
        public Guid Id { get; set; }
        public IList<ScanDataPoint> List { get; set; }
    }
}