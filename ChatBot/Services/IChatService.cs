namespace ChatBot.Services
{
    public interface IChatService
    {
        Task<string> GetChatResponse(string question);
    }
}
