using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointCloudWeb.Server.Models
{
    public class PointCloudDto
    {
        public PointCloudDto(Guid id, string name, IList<Point> points)
        {
            Id = id;
            Name = name;
            Points = points;
        }

        public Guid Id { get; }
        public string Name { get; }
        public IList<Point> Points { get; }
    }
}