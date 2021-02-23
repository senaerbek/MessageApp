using System;

namespace SignalR.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string MessageContent { get; set; }
        public DateTime Date { get; set; }
        public Room Room { get; set; }
    }
}