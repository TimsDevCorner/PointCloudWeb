using System;
using System.Collections.Generic;

namespace PointCloudWeb.Server.Models
{
    public class PointCloudDto
    {
        public PointCloudDto(Guid id, IList<Point> points)
        {
            Id = id;
            Points = points;
        }

        public Guid Id { get; }
        public IList<Point> Points { get; }
    }
}