using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ChatHub : Hub
    {
        public string ConnId() => 
            Context.ConnectionId;
        
    }
}
