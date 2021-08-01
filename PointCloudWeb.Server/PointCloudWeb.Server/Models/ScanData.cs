using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PointCloudWeb.Server.Models
{
    public class ScanDataList
    {
        public Guid Id { get; set; }
        public IList<ScanDataPoint> ScanPoints { get; set; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
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

        public override string ToString()
        {
            return string.Join(
                ", ", 
                RAY.ToString(CultureInfo.InvariantCulture), 
                RAX.ToString(CultureInfo.InvariantCulture), 
                DistanceMM.ToString(CultureInfo.InvariantCulture));
        }
    }
}