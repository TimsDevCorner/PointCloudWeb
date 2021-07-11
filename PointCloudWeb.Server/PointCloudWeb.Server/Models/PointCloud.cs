using System;
using System.Collections.Generic;
using System.Linq;

namespace PointCloudWeb.Server.Models
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public Point() : this(0, 0, 0) { }
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }


        public override string ToString() => (X.ToString() + "  " + Y.ToString() + "  " + Z.ToString());
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