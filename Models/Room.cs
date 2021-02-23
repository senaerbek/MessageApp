using System.Collections.Generic;

namespace SignalR.Models
{
    public class Room
    {
        public Room()
        {
            Message = new List<Message>();
            User = new List<RoomUser>();
        }

        public int Id { get; set; }
        public string RoomName { get; set; }
        public ICollection<Message> Message { get; set; }
        public ICollection<RoomUser> User { get; set; }
    }
}