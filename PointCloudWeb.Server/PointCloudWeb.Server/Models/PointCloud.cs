using PointCloudWeb.Server.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

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
            if (obj == null || GetType() != obj.GetType())
                return false;
            var p = (Point) obj;
            return X == p.X && Y == p.Y && Z == p.Z;
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => X + "  " + Y + "  " + Z;
    }

    public class PointCloud
    {
        private readonly ObservableCollection<Point> _points;
        private Matrix4x4 _transformation;

        public PointCloud(Guid id, string name)
        {
            _points = new ObservableCollection<Point>();
            _points.CollectionChanged += PointsCollectionChanged;
            TransformedPoints = new List<Point>();
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public IList<Point> Points
        {
            get => _points;
        }

        public Matrix4x4 Transformation
        {
            get => _transformation;
            set
            {
                TransformationChanged();
                _transformation = value;
            }
        }

        public IList<Point> TransformedPoints { get; private set; }

        private Point GetTransformedPoint(Point point)
        {
            if (Transformation.IsIdentity)
                return new Point(point.X, point.Y, point.Z);

            var v = new Vector3(point.X, point.Y, point.Z);
            v = Vector3.Transform(v, Transformation);

            return new Point(NumericUtils.Round(v.X), NumericUtils.Round(v.Y), NumericUtils.Round(v.Z));
        }

        private void PointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (TransformedPoints.Count > 0)
                TransformationChanged();
        }

        public void TransformationChanged()
        {
            TransformedPoints.Clear();

            if (Transformation.IsIdentity)
                return;

            foreach (var point in Points)
            {
                var transformedPoint = GetTransformedPoint(point);
                TransformedPoints.Add(transformedPoint);
            }
        }
    }

    public class PointCloudCollection : List<PointCloud>
    {
        public PointCloud AddNew()
        {
            var id = Guid.NewGuid();
            var pc = new PointCloud(id, "Scan #" + (Count + 1).ToString());
            Add(pc);
            return pc;
        }

        public bool Contains(Guid id)
        {
            return GetById(id) != null;
        }

        public PointCloud GetById(Guid id)
        {
            return Find(pc => pc.Id == id);
        }

        public void RemoveById(Guid id)
        {
            Remove(GetById(id));
        }
    }
}