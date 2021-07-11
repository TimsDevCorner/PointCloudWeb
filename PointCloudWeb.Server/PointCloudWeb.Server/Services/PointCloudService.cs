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

        public void AddPoints(Guid id, IList<Point> points)
        {
            if (!_pointClouds.Contains(id))
                throw new ArgumentOutOfRangeException("The Id {0} was not found!", id.ToString());

            var pc = _pointClouds.GetById(id);

            foreach (var point in points)
                pc.Points.Add(point);
        }
    }
}