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
        private readonly ScanDataService _scanDataService;

        public DataController(ScanDataService scanDataService)
        {
            this._scanDataService = scanDataService;
        }

        [HttpPost]
        public void PostScanData([FromBody] ScanDataList data)
        {
            _scanDataService.AddScan(data);
        }

        [HttpPut]
        public void ScanFinished(Guid id)
        {
            _scanDataService.ScanFinished(id);
        }
    }
}