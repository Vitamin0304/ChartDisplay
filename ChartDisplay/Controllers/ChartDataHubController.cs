using ChartDisplay.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task Get(double time, double data)
        {
            await _hub.Clients.All.SendAsync("ReceiveData", time, data);
        }
    }
}
