using Microsoft.AspNetCore.Mvc;
using PointCloudWeb.Server.Models;
using PointCloudWeb.Server.Services;

namespace PointCloudWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly DataService _scanDataService;

        public DataController(DataService scanDataService)
        {
            _scanDataService = scanDataService;
        }

        [HttpPost]
        public void PostScanData([FromBody] ScanDataList data)
        {
            _scanDataService.AddScan(data);
        }
    }
}
