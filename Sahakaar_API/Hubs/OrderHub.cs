using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Sahakaar_API.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendNewOrderNotification(object orderData)
        {
            await Clients.All.SendAsync("ReceiveNewOrder", orderData);
        }
    }
}
