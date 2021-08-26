using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Utils;

// ReSharper disable PossibleMultipleEnumeration

namespace PointCloudWeb.Server.Services
{
    public class PointCloudRegistrationServiceTeaerPp : IPointCloudRegistrationService
    {
        public RegistrationResult RegisterPointCloud(PointCloud source, PointCloud target)
        {
            var sourceFileName = Globals.TempPath + $"/{source.Id}.ply";
            var targetFileName = Globals.TempPath + $"/{target.Id}.ply";

            var outputFileName = Globals.TempPath + $"/{target.Id}.txt";

            var maxPoints = 5_000;

            source.WriteToPly(sourceFileName, maxPoints);
            target.WriteToPly(targetFileName, maxPoints);

            var teaserPp = new Process();
            teaserPp.StartInfo.FileName = "wsl";
            teaserPp.StartInfo.Arguments =
                $"{Globals.ToWslPath(Globals.TeaserPp)} " +
                $"\"{Globals.ToWslPath(outputFileName)}\" " +
                $"\"{Globals.ToWslPath(sourceFileName)}\" " +
                $"\"{Globals.ToWslPath(targetFileName)}\" ";

            teaserPp.Start();
            teaserPp.WaitForExit();

            //Console.WriteLine($"RegistrationExitCode: {teaserPp.ExitCode}");

            var result = new RegistrationResult();

            // if (teaserPp.ExitCode != 0)
            //     return result;

            var lines = File.ReadAllLines(outputFileName).ToList();
            while (lines.Any(s => s.Contains("  ")))
                lines = lines.Select(s =>
                {
                    var newString = s.Replace("  ", " ");
                    if (newString.Length > 0 && newString[0] == ' ')
                        newString = newString.Remove(0, 1);
                    return newString;
                }).ToList();

            var rotationIndex = lines.FindIndex(s => s.Contains("Estimated rotation:"));
            var translationIndex = lines.FindIndex(s => s.Contains("Estimated translation:"));

            var ci = CultureInfo.InvariantCulture;

            var rotationMatrix = Matrix4x4.Identity;
            rotationMatrix.M11 = float.Parse(lines[rotationIndex + 1].Split(' ')[0], ci);
            rotationMatrix.M12 = float.Parse(lines[rotationIndex + 1].Split(' ')[1], ci);
            rotationMatrix.M13 = float.Parse(lines[rotationIndex + 1].Split(' ')[2], ci);
            rotationMatrix.M21 = float.Parse(lines[rotationIndex + 2].Split(' ')[0], ci);
            rotationMatrix.M22 = float.Parse(lines[rotationIndex + 2].Split(' ')[1], ci);
            rotationMatrix.M23 = float.Parse(lines[rotationIndex + 2].Split(' ')[2], ci);
            rotationMatrix.M31 = float.Parse(lines[rotationIndex + 3].Split(' ')[0], ci);
            rotationMatrix.M32 = float.Parse(lines[rotationIndex + 3].Split(' ')[1], ci);
            rotationMatrix.M33 = float.Parse(lines[rotationIndex + 3].Split(' ')[2], ci);

            result.Rotation = MatrixUtils.RotationMatrixToAngles(rotationMatrix);

            var transformation = Vector3.Zero;
            transformation.X = float.Parse(lines[translationIndex + 1], ci);
            transformation.Y = float.Parse(lines[translationIndex + 2], ci);
            transformation.Z = float.Parse(lines[translationIndex + 3], ci);

            result.Transformation = transformation;

            return result;
        }
    }
}