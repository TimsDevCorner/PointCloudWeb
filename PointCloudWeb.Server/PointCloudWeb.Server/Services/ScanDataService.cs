using System;
using System.Collections.Generic;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class ScanDataService
    {
        private readonly PointCloudService _pointCloudService;
        private readonly ScanConverterService _scanConverterService;

        public ScanDataService(PointCloudService pointCloudService, ScanConverterService scanConverterService)
        {
            _pointCloudService = pointCloudService;
            _scanConverterService = scanConverterService;
        }

        private IList<Point> ConvertToPoints(ScanDataList scanData)
        {
            var list = new List<Point>();

            foreach (var scan in scanData.ScanPoints)
            {
                list.Add(_scanConverterService.Transform(scan));
            }

            return list;
        }

        public void AddScan(ScanDataList scanData)
        {
            _pointCloudService.AddPoints(scanData.Id, ConvertToPoints(scanData));
        }

        public void ScanFinished(Guid id)
        {
            _pointCloudService.RegisterPointCloud(id);
        }
    }
}