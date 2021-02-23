using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Context;
using SignalR.Hubs;
using SignalR.Models;

namespace SignalR.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> _chatHub;
        private SignalRDbContext _context { get; }
        public ChatController(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> Join(string connectionId, string roomName){
            await _chatHub.Groups.AddToGroupAsync(connectionId, roomName);
            return Ok();
        }

        [HttpPost("[action]")]
         public async Task<IActionResult> SendMessage(int roomId, 
         string message, string roomName, [FromServices] SignalRDbContext _ctxt){
           
             var messageAdd = new Message
            {
                MessageContent = message,
                RoomId = roomId,
                Name = User.Identity.Name,
                Date = DateTime.Now
            };
            _ctxt.Messages.Add(messageAdd);
            await _ctxt.SaveChangesAsync();
            await _chatHub.Clients.Group(roomName).SendAsync("RecieveMessage", 
            new{
                MessageContent = messageAdd.MessageContent,
                Name = messageAdd.Name,
                Date = messageAdd.Date 
            });
            return Ok();
        }
    }
}