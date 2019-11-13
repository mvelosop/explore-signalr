using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IHostApplicationLifetime _lifetime;

        public ChatHub(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public async Task Command(string user, string message)
        {
            if (message.Equals("shutdown", StringComparison.OrdinalIgnoreCase))
            {
                await SendMessage("Server", $"{user} sent a command to stop the server! 😲");

                _lifetime.StopApplication();

                return;
            }

            await SendMessage("Server", $"{user} sent command \"{message}\" but I don't know what to do with it 🤔");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}