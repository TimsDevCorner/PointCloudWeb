using System;
using System.Collections.Generic;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class DataService
    {
        private readonly PointCloudService pointCloudService;
        private readonly ScanConverterService scanConverterService;

        public DataService(PointCloudService pointCloudService, ScanConverterService scanConverterService)
        {
            this.pointCloudService = pointCloudService;
            this.scanConverterService = scanConverterService;
        }

        private IList<Point> ConvertToPoints(ScanDataList scanData)
        {
            var list = new List<Point>();

            foreach (var scan in scanData.ScanPoints)
            {
                list.Add(scanConverterService.Transform(scan));
            }

            return list;
        }

        public void AddScan(ScanDataList scanData)
        {
            pointCloudService.AddPoints(scanData.Id, ConvertToPoints(scanData));
        }

        public void ScanFinished(Guid id)
        {
            pointCloudService.RegisterPointCloud(id);
        }
    }
}