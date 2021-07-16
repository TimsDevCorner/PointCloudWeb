using System;
using System.Collections.Generic;
using System.Linq;

namespace PointCloudWeb.Server.Models
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }


        public Point() : this(0, 0, 0) { }
        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

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


        public override string ToString() => (X.ToString() + "  " + Y.ToString() + "  " + Z.ToString());
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    }


    public class PointCloud
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public IList<Point> Points { get; private set; }

        /// <summary>
        ///     Function <c>ScaleFactor</c> defines relation between X/Y/Z distances in mm
        ///     So, Point A(0,0,0) is 1mm from B(1,0,0) apart
        /// </summary>
        public static int ScaleFactor() => 1;

        public PointCloud(String name, Guid id)
        {
            Points = new List<Point>();
            Id = id;
            Name = name;
        }
    }

    public class PointCloudCollection : List<PointCloud>
    {
        public PointCloud GetById(Guid id)
        {
            return this.First(pc => pc.Id == id);
        }

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

        public void RemoveById(Guid id)
        {
            Remove(GetById(id));
        }
    }
}