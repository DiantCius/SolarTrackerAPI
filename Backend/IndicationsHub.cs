using Backend.DataAccess;
using Backend.DTO;
using Backend.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class IndicationsHub : Hub
    {

        public Task JoinGroup(string serialNumber)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, serialNumber);
        }

        /*public async Task SendMessageToGroup(string serialNumber)
        {
            var indication = await _indicationService.GetIndication(serialNumber, new CancellationToken());
            await Clients.Group(serialNumber).SendAsync("ReceiveIndication", indication);
        }*/


    }
}
