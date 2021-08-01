using System;
using System.Collections.Generic;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class PointCloudService
    {
        //private readonly IPointCloudRegistationService pointCloudRegistation;
        private readonly PointCloudCollection _pointClouds;

        public PointCloudService(/*IPointCloudRegistationService pointCloudRegistation*/)
        {
            _pointClouds = new PointCloudCollection();
            //this.pointCloudRegistation = pointCloudRegistation;
            InitSampleData();
        }

        private void InitSampleData()
        {
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 1"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 2"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 3"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 4"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 5"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 6"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 7"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 8"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 9"));
            _pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 10"));
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

            //var transformation = pointCloudRegistation.RegisterPointCloud(pointCloud, pointClouds[0]);
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