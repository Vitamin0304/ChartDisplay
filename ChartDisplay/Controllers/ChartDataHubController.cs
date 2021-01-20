using ChartDisplay.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartDisplay.Models;

namespace ChartDisplay.Controllers
{
    [ApiController]
    [Route("api/chart")]
    public class ChartDataHubController : ControllerBase
    {
        private readonly IHubContext<ChartDataHub> _hub;

        public ChartDataHubController(IHubContext<ChartDataHub> hub)
        {
            _hub = hub;
        }

        [HttpPost]
        public async Task<ActionResult<ChartDisplayData>> PostChartData(ChartDisplayData chartDisplayData)
        {
            await _hub.Clients.All.SendAsync("ReceiveChartDisplayData", chartDisplayData);
            return chartDisplayData;
        }
    }
}
