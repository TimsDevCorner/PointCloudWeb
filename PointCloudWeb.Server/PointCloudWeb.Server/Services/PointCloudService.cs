using System;
using System.Collections.Generic;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class PointCloudService
    {
        private readonly PointCloudCollection _pointClouds;

        public PointCloudService()
        {
            _pointClouds = new PointCloudCollection();
        }

        private void RaiseIfNotExists(Guid id)
        {
            if (!_pointClouds.Contains(id))
                throw new ArgumentOutOfRangeException("The Id {0} was not found!", id.ToString());
        }

        public void AddPoints(Guid id, IList<Point> points)
        {
            RaiseIfNotExists(id);

            var pc = _pointClouds.GetById(id);

            foreach (var point in points)
                pc.Points.Add(point);
        }

        public void RegisterPointCloud(Guid id)
        {
            RegisterPointClouds(new List<Guid>() { id });
        }

        public void RegisterPointClouds(IList<Guid> ids)
        {
            //ensure that every element in "ids" is in "_pointClouds"
            foreach (var id in ids)
                RaiseIfNotExists(id);

            throw new NotImplementedException();
        }
    }
}