using Microsoft.AspNetCore.Mvc;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PointCloudWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointCloudController
    {
        private readonly PointCloudService pointCloudService;

        public PointCloudController(PointCloudService pointCloudService)
        {
            this.pointCloudService = pointCloudService;
        }

        private PointCloudDto ConvertPointCloudToDto(PointCloud pc)
        {
            return new PointCloudDto(pc.Id, pc.Name, pc.TransformedPoints);
        }

        [HttpGet]
        public IList<PointCloudDto> GetAll()
        {
            var result = new List<PointCloudDto>();
            foreach (var pc in pointCloudService.GetAll())
                result.Add(ConvertPointCloudToDto(pc));

            return result;
        }

        [HttpGet]
        public PointCloudDto GetById(Guid id)
        {
            var pc = pointCloudService.GetById(id) ?? throw new KeyNotFoundException();
            return ConvertPointCloudToDto(pc);
        }
    }
}