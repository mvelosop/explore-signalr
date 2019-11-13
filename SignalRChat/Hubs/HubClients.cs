using System.Collections.Concurrent;

namespace SignalRChat.Hubs
{
    public class HubClients : ConcurrentDictionary<string, ClientEntry>
    {
    }
}