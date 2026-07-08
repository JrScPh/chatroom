namespace ChatroomAPI.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
    }
}
