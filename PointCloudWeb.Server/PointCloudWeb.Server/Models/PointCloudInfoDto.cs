using System;

namespace PointCloudWeb.Server.Models
{
    public class PointCloudInfoDto
    {
        public PointCloudInfoDto() : this(Guid.Empty, "")
        {
        }

        public PointCloudInfoDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}