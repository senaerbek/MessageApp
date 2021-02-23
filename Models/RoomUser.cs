namespace SignalR.Models
{
    public class RoomUser{
        public string UserId { get; set; }
        public User User { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
        public Role Role { get; set; }
    }
}