namespace ChatroomAPI.DTOs
{
    public class CreateMessageDto
    {
        public string Nickname { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
