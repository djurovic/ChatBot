namespace ChatBot.Models.DTOs
{
    public class ChatMessageDTO
    {
        public string UserId { get; set; }
        public string Question { get; set; }
        public string PrettifiedResponse { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
