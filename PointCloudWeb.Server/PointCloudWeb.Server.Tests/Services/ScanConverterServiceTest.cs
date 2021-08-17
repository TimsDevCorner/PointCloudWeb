using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;
using Xunit;

namespace PointCloudWeb.Server.Tests.Services
{
    public class ScanConverterServiceTest
    {
        [Theory]
        [InlineData(2, 2, 2)]
        [InlineData(2, 1, 1)]
        [InlineData(1, 2, 1)]
        [InlineData(1, 1, 2)]
        [InlineData(10, 15, 32)]
        [InlineData(5, 12, 7)]

        //Z = 0 is not possible due to lack of Information
        [InlineData(0, 0, 0)]
        [InlineData(0, 0, 1)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 1, 1)]
        [InlineData(-1, 1, 1)]
        [InlineData(1, -1, 1)]
        [InlineData(-1, -1, 1)]
        [InlineData(-5, 12, 7)]
        [InlineData(-5, -12, 7)]
        [InlineData(-22, 13, 11)]
        [InlineData(0, 0, -1)]
        [InlineData(0, -1, -1)]
        [InlineData(-1, 0, -1)]
        [InlineData(-1, -1, -1)]
        [InlineData(1, 1, -1)]
        [InlineData(5, 12, -7)]
        [InlineData(-1, 1, -1)]
        [InlineData(1, -1, -1)]
        [InlineData(-5, 12, -7)]
        [InlineData(-5, -12, -7)]
        public static void ScanConverterTest(int x, int y, int z)
        {
            var scan = new ScanDataPoint
            {
                RAX = Math.Acos(y / Math.Sqrt(Math.Pow(y, 2) + Math.Pow(z, 2))),
                RAY = Math.Acos(x / Math.Sqrt(Math.Pow(z, 2) + Math.Pow(x, 2))),
                DistanceMM = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2))
            };

            if (double.IsNaN(scan.RAX))
                scan.RAX = 0;
            if (double.IsNaN(scan.RAY))
                scan.RAY = 0;

            //from rad to degree
            scan.RAX = scan.RAX * 180 / Math.PI;
            scan.RAY = scan.RAY * 180 / Math.PI;

            if (z < 0)
            {
                scan.RAX += 270;
                scan.RAY += 270;
            }

            var expected = new Point(x, y, z);

            var point = ScanConverterService.Transform(scan);
            Assert.Equal(expected, point);
        }
    }
}