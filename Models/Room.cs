namespace ChatroomAPI.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
