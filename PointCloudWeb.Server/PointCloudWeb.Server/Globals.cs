using System.IO;

namespace PointCloudWeb.Server
{
    public static class Globals
    {
        static Globals()
        {
            var basePath = Directory.GetCurrentDirectory() + "/../..";
            PotreeDataPath = basePath + "/PointCloudWeb.Web/public/Potree/pointclouds/generated";
            PotreeConverterExe = basePath + "/PointCloudWeb.Server/Tools/PotreeConverter/PotreeConverter.exe";
            TempPath = basePath + "/temp";
            CloudCompareExe = "C:/Program Files/CloudCompare/CloudCompare.exe";
        }

        public static string PotreeDataPath { get; }
        public static string PotreeConverterExe { get; }
        public static string TempPath { get; }
        public static string CloudCompareExe { get; }
    }
}