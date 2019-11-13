using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly HubClients _clients;
        private readonly IHostApplicationLifetime _lifetime;

        public ChatHub(
            IHostApplicationLifetime lifetime,
            HubClients clients)
        {
            _lifetime = lifetime;
            _clients = clients;
        }

        public async Task Command(string user, string message)
        {
            switch (message.ToLower())
            {
                case "shutdown":
                    await SendMessage("Server", $"{user} sent a SHUTDOWN command! 😲");
                    _lifetime.StopApplication();

                    return;

                case "list":
                    var clients = _clients.Select(e => e.Value.ConnectionId).ToList();
                    var clientsMessage = $"{user} sent a LIST command. There are {clients.Count} clients: 😊<br/>- {string.Join("<br/>- ", clients)}";
                    await SendMessage("Server", clientsMessage);

                    return;
            }

            await SendMessage("Server", $"{user} sent command \"{message}\" but I don't know what to do with it 🤔");
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var clientEntry = new ClientEntry(Context);

            _clients[clientEntry.ConnectionId] = clientEntry;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            _clients.TryRemove(Context.ConnectionId, out _);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}