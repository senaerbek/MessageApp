using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SignalR.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SignalR.Components
{
    [ViewComponent(Name = "RoomViewComponent")]
    public class RoomViewComponent : ViewComponent
    {
        
        private SignalRDbContext _context;
        public RoomViewComponent(SignalRDbContext context)
        {
            _context = context;
        }
        public  IViewComponentResult Invoke(){
            var roomList =  _context.RoomUsers
            .Include(x=>x.Room)
            .Where(w=>w.UserId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
            .Select(x=>x.Room).ToList();
            return View(roomList);
        }
    }
}