using System;
using System.Collections.Generic;
using System.Linq;
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

        private static IEnumerable<Point> ConvertToPoints(ScanDataList scanData)
            => scanData.ScanPoints.Select(ScanConverterService.Transform).ToList();

        public void AddScanData(ScanDataList scanData)
            => _pointCloudService.AddPoints(scanData.Id, ConvertToPoints(scanData));

        public void ScanFinished(Guid id)
            => _pointCloudService.PointCloudCompleted(id);
    }
}