using System;
using System.Diagnostics;
using System.Linq;
using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public class PointCloudRegistrationServiceTeaerPp : IPointCloudRegistrationService
    {
        public RegistrationResult RegisterPointCloud(PointCloud source, PointCloud target)
        {
            var sourceFileName = Globals.TempPath + $"/{source.Id}.ply";
            var targetFileName = Globals.TempPath + $"/{target.Id}.ply";

            var maxPoints = 5_000;

            source.WriteToPly(sourceFileName, maxPoints);
            target.WriteToPly(targetFileName, maxPoints);

            var teaserPp = new Process();
            teaserPp.StartInfo.FileName = "C:\\Windows\\System32\\wsl.exe";
            // teaserPp.StartInfo.FileName = "wsl";
            teaserPp.StartInfo.Arguments = "curl https://www.google.de";
            // teaserPp.StartInfo.Arguments = $"\"{sourceFileName}\" \"{targetFileName}\"";
            //teaserPp.StartInfo.Arguments = $" ls "; //{Globals.ToWslPath(Globals.TeaserPp)} ";// +
            //"\"/mnt/c/Users/timwu/source/repos/PointCloudWeb/PointCloudWeb.Server/Tools/TEASERpp/c4b9b7fc-0b97-4f52-ad1b-737aeca5ba97.ply\" " +
            //"\"/mnt/c/Users/timwu/source/repos/PointCloudWeb/PointCloudWeb.Server/Tools/TEASERpp/c620b175-ace8-42e5-bf29-55b6c99372bc.ply\" ";
            
            teaserPp.StartInfo.RedirectStandardOutput = true;
            teaserPp.Start();


            var output = teaserPp.StandardOutput.ReadToEnd();
            Console.WriteLine(output[..Math.Min(output.Length, 100)]);
            
            
            teaserPp.WaitForExit();

            Console.WriteLine($"RegistrationExitCode: {teaserPp.ExitCode}");

            var result = new RegistrationResult();

            return result;
        }
    }
}