using ChatroomAPI.Models;

namespace ChatroomAPI.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
