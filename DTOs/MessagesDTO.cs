namespace ChatroomAPI.DTOs
{
    public class MessagesDTO
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
