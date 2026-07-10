using ChatroomAPI.Data;
using ChatroomAPI.DTOs;
using ChatroomAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatroomAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(int roomId, string nickname, string content)
        {
            var message = new Message
            {
                RoomId = roomId,
                Nickname = nickname,
                Content = content,
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", new MessageDTO
            {
                Id = message.Id,
                Nickname = message.Nickname,
                Content = message.Content,
                Timestamp = message.Timestamp
            });
        }
    }
}