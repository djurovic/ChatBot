namespace ChatBot.Models
{
    public class ChatResponse
    {
        public string model { get; set; }
        public DateTime created_at { get; set; }
        public Message message { get; set; }
        public string done_reason { get; set; }
        public bool done { get; set; }
        public long total_duration { get; set; }
        public long load_duration { get; set; }
        public int prompt_eval_count { get; set; }
        public long prompt_eval_duration { get; set; }
        public int eval_count { get; set; }
        public long eval_duration { get; set; }
    }
}
