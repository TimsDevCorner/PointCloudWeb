using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;
using Xunit;

namespace PointCloudWeb.Server.Tests.Services
{
    public class ScanConverterServiceTest
    {

        [Theory]
        [InlineData(1, 1, 1)]
        //[InlineData(-1, 1, 1)]
        //[InlineData(1, -1, 1)]
        //[InlineData(1, 1, -1)]
        //[InlineData(-1, -1, 1)]
        //[InlineData(-1, 1, -1)]
        //[InlineData(1, -1, -1)]


        //[InlineData(10, 15, 32)]
        //[InlineData(5, 12, 7)]
        //[InlineData(-5, 12, 7)]
        //[InlineData(5, -12, 7)]
        //[InlineData(5, 12, -7)]
        //[InlineData(-5, -12, 7)]
        //[InlineData(-5, 12, -7)]
        //[InlineData(-5, -12, -7)]
        //[InlineData(-22, 13, 11)]
        public static void ScanConverterTest(int x, int y, int z)
        {
            var scan = new ScanDataPoint
            {
                RAX = 45,//(Math.Acos(y / Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2))) * 180 / Math.PI),
                RAY = 45,//(Math.Acos(z / Math.Sqrt(Math.Pow(z, 2) + Math.Pow(x, 2))) * 180 / Math.PI),
                //RAY = 360 - (Math.Acos(z / Math.Sqrt(Math.Pow(z, 2) + Math.Pow(x, 2))) * 180 / Math.PI),
                //RAY = 180 - (Math.Acos(z / Math.Sqrt(Math.Pow(z, 2) + Math.Pow(x, 2))) * 180 / Math.PI),
                DistanceMM = (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2))
            };

            //if (scan.RAX >= 0 && scan.RAX < 90)
            //    scan.RAY = 360 - scan.RAY;
            //else if (scan.RAX >= 90 && scan.RAX < 180)
            //    scan.RAY = 180 - scan.RAY;

            //if (scan.RAX < 270 && scan.RAY >= 270 && scan.RAY < 360)
            //    scan.RAX = 360 - scan.RAX;
            //else if (angle >= 180 && angle < 270)
            //{
            //    sin *= -1;
            //    cos *= -1;
            //}
            //else if (angle >= 270 && angle < 360)
            //{
            //    sin *= -1;
            //}

            var expected = new Point(x, y, z);

            var service = new ScanConverterService();
            var point = service.Transform(scan);
            //Assert.Equal(expected, point);
            Assert.True(
                point.X >= expected.X - 1 && point.X <= expected.X + 1
                    && point.Y >= expected.Y - 1 && point.Y <= expected.Y + 1
                    && point.Z >= expected.Z - 1 && point.Z <= expected.Z + 1,
                    "expected: " + expected.ToString() + " \nactual:   " + point.ToString() +
                    "\nAX: " + scan.RAX + "\n AY: " + scan.RAY
                    );
        }
    }
}
