namespace ChatroomAPI.DTOs
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
