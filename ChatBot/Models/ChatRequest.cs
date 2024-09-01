namespace ChatBot.Models
{
    public class ChatRequest
    {
        public string model;
        public Message[] messages;
        public bool stream;
    }
}
