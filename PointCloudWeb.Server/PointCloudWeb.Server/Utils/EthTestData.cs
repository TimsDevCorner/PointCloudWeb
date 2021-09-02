using System;
using System.Globalization;
using System.IO;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;

namespace PointCloudWeb.Server.Utils
{
    public static class EthTestData
    {
        private static void LoadPointCloudFromEthFile(PointCloud target, string path)
        {
            var lines = File.ReadLines(path);
            foreach (var line in lines)
            {
                //skip header
                if (line.Contains("x,y,z"))
                    continue;

                var values = line.Split(',');

                var point = new Point(
                    TextToCoordinate(values[1]),
                    TextToCoordinate(values[2]),
                    TextToCoordinate(values[3])
                );
                target.Points.Add(point);
            }
        }

        private static int TextToCoordinate(string text)
        {
            var dDecimal = Convert.ToDecimal(text, CultureInfo.InvariantCulture);
            dDecimal *= 1_000; //convert decimal points to integer points
            return (int)dDecimal;
        }

        public static void CreateData(PointCloudService pointCloudService)
        {
            // var pc = pointCloudService.AddPointCloud(new Guid("c4b9b7fc-0b97-4f52-ad1b-737aeca5ba97"));
            // LoadPointCloudFromEthFile(pc, "ETH-Data/Hokuyo_0.csv");
            // pointCloudService.PointCloudCompleted(pc.Id);
            //
            // pc = pointCloudService.AddPointCloud(new Guid("c620b175-ace8-42e5-bf29-55b6c99372bc"));
            // LoadPointCloudFromEthFile(pc, "ETH-Data/Hokuyo_1.csv");
            // pointCloudService.PointCloudCompleted(pc.Id);
        }
    }
}