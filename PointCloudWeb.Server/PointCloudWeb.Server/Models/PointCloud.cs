using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

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
            var p = (Point)obj;
            return X == p.X && Y == p.Y && Z == p.Z;
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override string ToString() => X + "  " + Y + "  " + Z;
    }

    public class PointCloud
    {
        public PointCloud(Guid id, string name)
        {
            Points = new List<Point>();
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public IList<Point> Points { get; }

        public Vector3 Transformation { get; set; }
        public Vector3 Rotation { get; set; }


        // ReSharper disable once MemberCanBePrivate.Global
        public string ToStringXyz()
        {
            var stringBuilder = new StringBuilder();

            // var maxPoints = 5_000;

            var takeEvery = 1;//Points.Count / maxPoints;

            for (var i = 0; i < Points.Count; i++)
            {
                if (i % takeEvery != 0)
                    continue;
                var point = Points[i];
                stringBuilder.AppendLine(string.Join(',',
                        point.X.ToString(CultureInfo.InvariantCulture),
                        point.Y.ToString(CultureInfo.InvariantCulture),
                        point.Z.ToString(CultureInfo.InvariantCulture)
                    )
                );
            }

            return stringBuilder.ToString();
        }

        private string ToStringPly(int maxPoints)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("ply\n");
            stringBuilder.Append("format ascii 1.0\n");

            stringBuilder.Append("element vertex COUNT_PLACEHOLDER\n");
            stringBuilder.Append("property float x\n");
            stringBuilder.Append("property float y\n");
            stringBuilder.Append("property float z\n");
            stringBuilder.Append("end_header\n");

            if (maxPoints == 0)
                maxPoints = Points.Count;

            var takeEvery = Points.Count / maxPoints;
            if (takeEvery < 1)
                takeEvery = 1;
            var count = 0;

            for (var i = 0; i < Points.Count; i++)
            {
                if (i % takeEvery != 0)
                    continue;
                count += 1;
                var point = Points[i];
                stringBuilder.Append(string.Join(' ',
                        point.X,
                        point.Y,
                        point.Z
                    ) + "\n"
                );
            }

            stringBuilder.Replace("COUNT_PLACEHOLDER", count.ToString());

            return stringBuilder.ToString();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void WriteToXyz(string fileName)
        {
            File.WriteAllText(fileName, ToStringXyz());
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void WriteToPly(string fileName, int maxPoints)
        {
            File.WriteAllText(fileName, ToStringPly(maxPoints));
        }

        public void WriteToLasCloudCompare(string fileName)
        {
            var fileNameXyz = Path.ChangeExtension(fileName, ".xyz");
            WriteToXyz(fileNameXyz);

            var cloudCompare = new Process();
            cloudCompare.StartInfo.FileName = Globals.CloudCompareExe;
            cloudCompare.StartInfo.Arguments =
                $"-SILENT -O \"{fileNameXyz}\" -C_EXPORT_FMT las -SAVE_CLOUDS FILE \"{fileName}\"";
            cloudCompare.Start();
            cloudCompare.WaitForExit();
        }

        public void WriteToLas(string fileName)
        {
            var fileNameXyz = Path.ChangeExtension(fileName, ".xyz");
            WriteToXyz(fileNameXyz);

            var txt2Las = new Process();
            txt2Las.StartInfo.FileName = Globals.LasToolsTxt2Las;
            txt2Las.StartInfo.Arguments =
                $"\"{fileNameXyz}\" -parse xyz -o \"{fileName}\"";
            txt2Las.Start();
            txt2Las.WaitForExit();
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

        public bool Contains(Guid? id)
        {
            return GetById(id) != null;
        }

        public PointCloud GetById(Guid? id)
        {
            return id == null ? null : Find(pc => pc.Id == id);
        }

        public void RemoveById(Guid id)
        {
            Remove(GetById(id));
        }
    }
}