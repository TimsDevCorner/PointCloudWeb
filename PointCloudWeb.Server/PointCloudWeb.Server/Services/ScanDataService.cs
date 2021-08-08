using System;
using System.Collections.Generic;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class ScanDataService
    {
        private readonly PointCloudService _pointCloudService;

        public ScanDataService(PointCloudService pointCloudService)
        {
            _pointCloudService = pointCloudService;
        }

        private IList<Point> ConvertToPoints(ScanDataList scanData)
        {
            var list = new List<Point>();

            foreach (var scan in scanData.ScanPoints)
            {
                list.Add(ScanConverterService.Transform(scan));
            }

            return list;
        }

        public void AddScan(ScanDataList scanData)
        {
            _pointCloudService.AddPoints(scanData.Id, ConvertToPoints(scanData));
        }

        public void ScanFinished(Guid id)
        {
            _pointCloudService.PointCloudCompleted(id);
        }
    }
}