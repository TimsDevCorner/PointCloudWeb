using Microsoft.AspNetCore.Mvc;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;

namespace PointCloudWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScanDataController : ControllerBase
    {
        private readonly ScanDataService _scanDataService;

        public ScanDataController(ScanDataService scanDataService)
        {
            _scanDataService = scanDataService;
        }

        [HttpPut]
        public void SendScanData([FromBody] ScanDataList data)
        {
            _scanDataService.AddScanData(data);
        }

        [HttpPut]
        [Route("finished/{id:Guid}")]
        public void ScanFinished(Guid id)
        {
            _scanDataService.ScanFinished(id);
        }
    }
}