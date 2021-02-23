using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SignalR.Models
{
    public class User : IdentityUser
    {
        public ICollection<RoomUser> Rooms { get; set; }
    }
}