using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ClientEntry
    {
        public ClientEntry(HubCallerContext context)
        {
            ConnectionId = context.ConnectionId;
            UserdIdentifier = context.UserIdentifier;
        }

        public string ConnectionId { get; set; }

        public string UserdIdentifier { get; set; }
    }
}