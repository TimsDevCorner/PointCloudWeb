using Microsoft.AspNetCore.Mvc;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;
using System.Collections.Generic;

namespace PointCloudWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointCloudController
    {
        private readonly PointCloudService _pointCloudService;

        public PointCloudController(PointCloudService pointCloudService)
        {
            this._pointCloudService = pointCloudService;
        }

        private PointCloudDto ConvertPointCloudToDto(PointCloud pc) => new PointCloudDto(pc.Id, pc.TransformedPoints);

        [HttpGet]
        public IList<PointCloudDto> GetAll()
        {
            var result = new List<PointCloudDto>();
            foreach (var pc in _pointCloudService.GetAll())
                result.Add(ConvertPointCloudToDto(pc));

            return result;
        }

        [HttpGet]
        public PointCloudDto GetById(Guid id)
        {
            var pc = _pointCloudService.GetById(id) ?? throw new KeyNotFoundException();
            return ConvertPointCloudToDto(pc);
        }
    }
}