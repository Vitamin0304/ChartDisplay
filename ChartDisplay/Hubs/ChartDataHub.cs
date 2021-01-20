using ChartDisplay.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChartDisplay.Hubs
{
    public class ChartDataHub : Hub
    {
        public async Task PostChartData(ChartDisplayData chartDisplayData)
        {
            await Clients.All.SendAsync("ReceiveChartDisplayData", chartDisplayData);
        }
    }
}