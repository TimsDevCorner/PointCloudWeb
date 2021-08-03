using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{   
    public class PointCloudService
    {
        //private readonly IPointCloudRegistrationService pointCloudRegistration;
        private readonly PointCloudCollection _pointClouds;

        public PointCloudService(/*IPointCloudRegistrationService pointCloudRegistration*/)
        {
            _pointClouds = new PointCloudCollection();
            //this.pointCloudRegistration = pointCloudRegistration;
            InitSampleData();
        }

        private static int TextToCoordinate(string text)
        {
            var dDecimal = Convert.ToDecimal(text, CultureInfo.InvariantCulture);
            dDecimal *= 100_000_000; //convert decimal points to integer points
            return (int)dDecimal;
        } 
        
        private static void LoadPointCloudFromEthFile(PointCloud target, string path)
        {
            var lines = File.ReadLines(path );
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

        private void ConvertPointsToPotree()
        {
            var path = Globals.PotreeDataPath;
        }
        
        private void InitSampleData()
        {
            var pc = new PointCloud(Guid.NewGuid(), "Scan 1");
            LoadPointCloudFromEthFile(pc, "ETH-Data/Hokuyo_0.csv");
            _pointClouds.Add(pc);
            
            pc = new PointCloud(Guid.NewGuid(), "Scan 2");
            LoadPointCloudFromEthFile(pc, "ETH-Data/Hokuyo_1.csv");
            _pointClouds.Add(pc);
        }

        private void RaiseIfNotExists(Guid id)
        {
            if (!_pointClouds.Contains(id))
                throw new ArgumentOutOfRangeException($"The Id {id.ToString()} was not found!");
        }

        public void AddPoints(Guid id, IList<Point> points)
        {
            RaiseIfNotExists(id);

            var pc = _pointClouds.GetById(id);

            foreach (var point in points)
                pc.Points.Add(point);
        }

        public IList<PointCloud> GetAll()
        {
            return _pointClouds;
        }

        public PointCloud GetById(Guid id)
        {
            return _pointClouds.GetById(id);
        }

        public void RegisterPointCloud(Guid id)
        {
            RaiseIfNotExists(id);
            // var pointCloud = _pointClouds.GetById(id);
            //
            // //the first can't be registered
            // if (_pointClouds.IndexOf(pointCloud) == 0)
            //     return;

            //var transformation = pointCloudRegistration.RegisterPointCloud(pointCloud, pointClouds[0]);
            //pointCloud.Transformation = transformation;
        }

        public void RegisterPointClouds()
        {
            foreach (var pointCloud in _pointClouds)
                RegisterPointCloud(pointCloud.Id);
        }

        public void RemoveById(Guid id)
        {
            _pointClouds.RemoveById(id);
        }
    }
}