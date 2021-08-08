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
            var pc = pointCloudService.AddPointCloud();
            pc.Name = "Scan 1";
            LoadPointCloudFromEthFile(pc, "ETH-Data/Hokuyo_0.csv");
            pointCloudService.PointCloudCompleted(pc.Id);

            pc = pointCloudService.AddPointCloud();
            pc.Name = "Scan 2";
            LoadPointCloudFromEthFile(pc, "ETH-Data/Hokuyo_1.csv");
            pointCloudService.PointCloudCompleted(pc.Id);
        }
    }
}