using System;
using System.Collections.Generic;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class PointCloudService
    {
        //private readonly IPointCloudRegistationService pointCloudRegistation;
        private readonly PointCloudCollection pointClouds;

        public PointCloudService(/*IPointCloudRegistationService pointCloudRegistation*/)
        {
            pointClouds = new PointCloudCollection();
            //this.pointCloudRegistation = pointCloudRegistation;
            InitSampleData();
        }

        private void InitSampleData()
        {
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 1"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 2"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 3"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 4"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 5"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 6"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 7"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 8"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 9"));
            pointClouds.Add(new PointCloud(Guid.NewGuid(), "Scan 10"));
        }

        private void RaiseIfNotExists(Guid id)
        {
            if (!pointClouds.Contains(id))
                throw new ArgumentOutOfRangeException("The Id {0} was not found!", id.ToString());
        }

        public void AddPoints(Guid id, IList<Point> points)
        {
            RaiseIfNotExists(id);

            var pc = pointClouds.GetById(id);

            foreach (var point in points)
                pc.Points.Add(point);
        }

        public IList<PointCloud> GetAll()
        {
            return pointClouds;
        }

        public PointCloud GetById(Guid id)
        {
            return pointClouds.GetById(id);
        }

        public void RegisterPointCloud(Guid id)
        {
            RaiseIfNotExists(id);
            var pointCloud = pointClouds.GetById(id);

            //the first can't be registered
            if (pointClouds.IndexOf(pointCloud) == 0)
                return;

            //var transformation = pointCloudRegistation.RegisterPointCloud(pointCloud, pointClouds[0]);
            //pointCloud.Transformation = transformation;
        }

        public void RegisterPointClouds()
        {
            foreach (var pointCloud in pointClouds)
                RegisterPointCloud(pointCloud.Id);
        }

        public void RemoveById(Guid id)
        {
            pointClouds.RemoveById(id);
        }
    }
}