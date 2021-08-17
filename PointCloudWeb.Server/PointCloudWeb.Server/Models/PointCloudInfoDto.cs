using System;
using System.Numerics;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace PointCloudWeb.Server.Models
{
    [Serializable]
    public class Vector3Ser
    {
        public Vector3Ser() : this(0, 0, 0)
        {
        }

        public Vector3Ser(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class PointCloudInfoDto
    {
        public PointCloudInfoDto() : this(Guid.Empty, "", Vector3.Zero, Vector3.Zero)
        {
        }

        public PointCloudInfoDto(Guid id, string name, Vector3 rotation, Vector3 transformation)
        {
            Id = id;
            Name = name;
            Rotation = new Vector3Ser(rotation.X, rotation.Y, rotation.Z);
            Transformation = new Vector3Ser(transformation.X, transformation.Y, transformation.Z);
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Vector3Ser Rotation { get; set; }
        public Vector3Ser Transformation { get; set; }
    }
}