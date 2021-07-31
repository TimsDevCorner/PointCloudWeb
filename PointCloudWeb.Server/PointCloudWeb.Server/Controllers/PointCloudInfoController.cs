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
    public class PointCloudInfoController
    {
        private readonly PointCloudService pointCloudService;

        public PointCloudInfoController(PointCloudService pointCloudService)
        {
            this.pointCloudService = pointCloudService;
        }

        private PointCloudInfoDto ConvertPointCloudToDto(PointCloud pc) => new PointCloudInfoDto(pc.Id, pc.Name);

        [HttpGet]
        public IList<PointCloudInfoDto> GetAll()
        {
            var result = new List<PointCloudInfoDto>();
            foreach (var pc in pointCloudService.GetAll())
                result.Add(ConvertPointCloudToDto(pc));

            return result;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public ActionResult<PointCloudInfoDto> GetById(Guid id)
        {
            var pc = pointCloudService.GetById(id);
            if (pc == null)
                return new NotFoundResult();
            return ConvertPointCloudToDto(pc);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public ActionResult RemoveById(Guid id)
        {
            if (pointCloudService.GetById(id) == null)
                return new NotFoundResult();

            pointCloudService.RemoveById(id);
            return new OkResult();
        }

        [HttpPut]
        public ActionResult<PointCloudInfoDto> UpdatePointCloud([FromBody] PointCloudInfoDto newPc)
        {
            var pc = pointCloudService.GetById(newPc.Id);
            if (pc == null)
                return new NotFoundResult();
            pc.Name = newPc.Name;
            return ConvertPointCloudToDto(pc);
        }
    }
}