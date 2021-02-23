using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalR.Context;
using SignalR.Models;

namespace SignalR.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SignalRDbContext _context { get; }
        public HomeController(SignalRDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Rooms.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetRoom(int id)
        {
            var getRoom = _context
            .Rooms
            .Include(a => a.Message)
            .FirstOrDefault(i => i.Id == id);
            return View(getRoom);
        }


        public async Task<IActionResult> SendMessage(int roomId, string Message)
        {
            var message = new Message
            {
                MessageContent = Message,
                RoomId = roomId,
                Name = User.Identity.Name,
                Date = DateTime.Now
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetRoom", new { id = roomId });
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom(string roomName)
        {
            var room = new Room
            {
                RoomName = roomName
            };

            room.User.Add(new RoomUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = Role.Admin
            });

            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var userR = new RoomUser
            {
               
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Role = Role.RoomUser,
                RoomId = id
            };
            _context.RoomUsers.Add(userR);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetRoom","Home", new {id = id});
        }

    }
}
