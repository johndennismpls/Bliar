using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Bliar.Hubs
{

    [Authorize]
    public class BlazorChatSampleHub : Hub
    {
        public const string HubUrl = "/chat";
        private readonly IHttpContextAccessor _context;

        public BlazorChatSampleHub(IHttpContextAccessor context)
        {
            _context = context;
        }

        public async Task Broadcast(string username, string message)
        {
            var x = Context.User.Identity.Name;
            await Clients.All.SendAsync("Broadcast", x, message);
        }

        public override Task OnConnectedAsync()
        {
            var x = Context.User.Identity.Name;
            Console.WriteLine($"{Context.ConnectionId} connected");
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
            await base.OnDisconnectedAsync(e);
        }
    }
}
