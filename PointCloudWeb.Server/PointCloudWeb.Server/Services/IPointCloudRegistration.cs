using PointCloudWeb.Server.Models;

namespace PointCloudWeb.Server.Services
{
    public interface IPointCloudRegistation
    {
        public TransformationMatrix RegisterPointCloud(PointCloud pc1, PointCloud pc2)
        {
            return null;
        }

        class TransformationMatrix { }
    }
}