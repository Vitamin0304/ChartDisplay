using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChartDisplay.Hubs
{
    public class ChartDataHub : Hub
    {
        public async Task SendMessage(double time, double data)
        {
            await Clients.All.SendAsync("ReceiveData", time, data);
        }
    }
}