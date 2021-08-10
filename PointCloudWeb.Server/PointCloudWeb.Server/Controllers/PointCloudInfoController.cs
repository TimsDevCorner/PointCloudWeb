using Microsoft.AspNetCore.Mvc;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;
using System.Collections.Generic;

namespace PointCloudWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointCloudInfoController
    {
        private readonly PointCloudService _pointCloudService;

        public PointCloudInfoController(PointCloudService pointCloudService)
        {
            _pointCloudService = pointCloudService;
        }

        private PointCloudInfoDto ConvertPointCloudToDto(PointCloud pc) => new PointCloudInfoDto(pc.Id, pc.Name);

        [HttpGet]
        public IList<PointCloudInfoDto> GetAll()
        {
            var result = new List<PointCloudInfoDto>();
            foreach (var pc in _pointCloudService.GetAll())
                result.Add(ConvertPointCloudToDto(pc));

            return result;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public ActionResult<PointCloudInfoDto> GetById(Guid id)
        {
            var pc = _pointCloudService.GetById(id);
            if (pc == null)
                return new NotFoundResult();
            return ConvertPointCloudToDto(pc);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public ActionResult RemoveById(Guid id)
        {
            if (_pointCloudService.GetById(id) == null)
                return new NotFoundResult();

            _pointCloudService.RemoveById(id);
            //Json Result, becaus OkResult throws in Firefox an XML-Root element not found error.
            return new JsonResult("OK");
        }

        [HttpPut]
        public ActionResult<PointCloudInfoDto> UpdatePointCloud([FromBody] PointCloudInfoDto newPc)
        {
            var pc = _pointCloudService.GetById(newPc.Id);
            if (pc == null)
                return new NotFoundResult();
            pc.Name = newPc.Name;
            return ConvertPointCloudToDto(pc);
        }
    }
}