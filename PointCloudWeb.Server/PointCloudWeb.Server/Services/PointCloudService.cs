using System;
using System.Collections.Generic;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class PointCloudService
    {
        private readonly IPointCloudRegistationService pointCloudRegistation;
        private readonly PointCloudCollection pointClouds;

        public PointCloudService(IPointCloudRegistationService pointCloudRegistation)
        {
            pointClouds = new PointCloudCollection();
            this.pointCloudRegistation = pointCloudRegistation;
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

        public void RegisterPointCloud(Guid id)
        {
            RaiseIfNotExists(id);
            var pointCloud = pointClouds.GetById(id);

            //the first can't be registered
            if (pointClouds.IndexOf(pointCloud) == 0)
                return;

            var transformation = pointCloudRegistation.RegisterPointCloud(pointCloud, pointClouds[0]);
            pointCloud.Transformation = transformation;
        }

        public void RegisterPointClouds()
        {
            foreach (var pointCloud in pointClouds)
                RegisterPointCloud(pointCloud.Id);
        }
    }
}