namespace ChatBot.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Question { get; set; }
        public string PrettifiedResponse { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
