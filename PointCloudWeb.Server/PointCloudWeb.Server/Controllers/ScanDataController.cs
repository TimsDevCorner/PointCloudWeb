using Microsoft.AspNetCore.Mvc;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;
using System;

namespace PointCloudWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ScanDataService scanDataService;

        public DataController(ScanDataService scanDataService)
        {
            this.scanDataService = scanDataService;
        }

        [HttpPost]
        public void PostScanData([FromBody] ScanDataList data)
        {
            scanDataService.AddScan(data);
        }

        [HttpPut]
        public void ScanFinished(Guid id)
        {
            scanDataService.ScanFinished(id);
        }
    }
}