using PointCloudWeb.Server.Models;
using System.Numerics;

namespace PointCloudWeb.Server.Services
{
    public interface IPointCloudRegistationService
    {
        public Matrix4x4 RegisterPointCloud(PointCloud source, PointCloud target);
    }
}