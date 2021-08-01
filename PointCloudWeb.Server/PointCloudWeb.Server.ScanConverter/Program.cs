using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PointCloudWeb.Server.ScanConverter
{
    internal class Program
    {
        private static void Main()
        {
            var scanPoints = File.ReadAllLines("C:\\Users\\timwu\\Desktop\\Scans\\0yGrad-edited-scan.csv")
                               .Select(v => ScanDataPointFromCsv(v))
                               .Where(scan => scan.DistanceMM > 0)
                               .ToList();

            var result = new List<Point>();
            var converter = new ScanConverterService();

            foreach (var scan in scanPoints)
            {
                result.Add(converter.Transform(scan));
                //Console.WriteLine(result.Count + "::  " + scan.ToString() + " => " + result[result.Count - 1].ToString());
            }

            result.RemoveAll(point => point.X == 0 && point.Y == 0 & point.Z == 0);

            string csv = String.Join("\n", result.Select(point => point.X + ", " + point.Y + ", " + point.Z).ToArray());

            File.WriteAllText("C:\\Users\\timwu\\Desktop\\Scans\\0yGrad-pc.csv", csv);

            Console.WriteLine("Convert finished");
            Console.ReadLine();
        }

        private static ScanDataPoint ScanDataPointFromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            return new ScanDataPoint(
                ray: Convert.ToDouble(values[0], CultureInfo.InvariantCulture),
                rax: Convert.ToDouble(values[1], CultureInfo.InvariantCulture),
                distanceMM: Convert.ToInt32(values[2], CultureInfo.InvariantCulture)
                );
        }
    }
}