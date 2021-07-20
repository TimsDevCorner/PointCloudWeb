using System;

namespace PointCloudWeb.Server.Utils
{
    public static class NumericUtils
    {
        public static int Round(double value) => (int)Math.Round(double.IsNaN(value) ? 0 : value, 0, MidpointRounding.AwayFromZero);
    }
}