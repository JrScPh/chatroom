using ChatroomAPI.Data;
using ChatroomAPI.DTOs;
using ChatroomAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatroomAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(new RoomDTO
            {
                Id = room.Id,
                Title = room.Title,
                Description = room.Description
            });
        }

        [HttpGet("{id}/messages")]
        public async Task<IActionResult> GetMessages(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            var messages = await _context.Messages.Where(m => m.RoomId == id).OrderBy(m => m.Timestamp).ToListAsync();
            var messageDtos = messages.Select(m => new MessagesDTO
            {
                Id = m.Id,
                Nickname = m.Nickname,
                Content = m.Content,
                Timestamp = m.Timestamp
            });
            return Ok(messageDtos);
        }

        [HttpPost("{id}/messages")]
        public async Task<IActionResult> PostMessage(int id, [FromBody] CreateMessageDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            var message = new Message
            {
                RoomId = id,
                Nickname = dto.Nickname,
                Content = dto.Content,
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return StatusCode(201, new MessagesDTO
            {
                Id = message.Id,
                Nickname = message.Nickname,
                Content = message.Content,
                Timestamp = message.Timestamp
            });
        }
    }
}