using System.Collections.Generic;
using System.Threading.Tasks;
using AngularMongoASP.SignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace AngularMongoASP.SignalR.HubConfig
{
    public class ChartHub: Hub
    {
        public async Task BroadcastChartData(List<ChartModel> data) => await Clients.All.SendAsync("broadcastchartdata", data);
    }
}