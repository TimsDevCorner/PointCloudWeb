using Microsoft.AspNetCore.Mvc;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;
using System.Collections.Generic;
using System.Numerics;

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

        private static PointCloudInfoDto ConvertPointCloudToDto(PointCloud pc) =>
            new PointCloudInfoDto(pc.Id, pc.Name, pc.Rotation, pc.Transformation);

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
            pc.Rotation = new Vector3(newPc.Rotation.X, newPc.Rotation.Y, newPc.Rotation.Z);
            pc.Transformation = new Vector3(newPc.Transformation.X, newPc.Transformation.Y, newPc.Transformation.Z);

            return ConvertPointCloudToDto(pc);
        }
    }
}