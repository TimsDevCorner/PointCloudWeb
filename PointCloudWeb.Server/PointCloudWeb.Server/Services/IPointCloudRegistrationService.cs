using PointCloudWeb.Server.Models;
using System.Numerics;

namespace PointCloudWeb.Server.Services
{
    public class RegistrationResult
    {
        public RegistrationResult()
        {
            Transformation = Vector3.Zero;
            Rotation = Vector3.Zero;
        }
        
        public Vector3 Transformation { get; set; }
        public Vector3 Rotation { get; set; }
    }
    public interface IPointCloudRegistrationService
    {
        public RegistrationResult RegisterPointCloud(PointCloud source, PointCloud target);
    }
}