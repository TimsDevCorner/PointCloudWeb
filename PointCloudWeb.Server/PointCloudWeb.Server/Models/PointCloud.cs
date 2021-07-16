using System;
using System.Collections.Generic;
using System.Linq;

namespace PointCloudWeb.Server.Models
{
    public class Point
    {
        public Point() : this(0, 0, 0)
        {
        }

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Point p = (Point)obj;
                return (X == p.X) && (Y == p.Y) && (Z == p.Z);
            }
        }

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => (X.ToString() + "  " + Y.ToString() + "  " + Z.ToString());
    }

    public class PointCloud
    {
        public PointCloud(String name, Guid id)
        {
            Points = new List<Point>();
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public IList<Point> Points { get; private set; }
    }

    public class PointCloudCollection : List<PointCloud>
    {
        public PointCloud AddNew()
        {
            var id = Guid.NewGuid();
            var pc = new PointCloud(id.ToString(), id);
            Add(pc);
            return pc;
        }

        public bool Contains(Guid id)
        {
            return this.Any(pc => pc.Id == id);
        }

        public PointCloud GetById(Guid id)
        {
            return this.First(pc => pc.Id == id);
        }

        public void RemoveById(Guid id)
        {
            Remove(GetById(id));
        }
    }
}