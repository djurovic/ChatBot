using ChatBot.Models.DTOs;

namespace ChatBot.Repository
{
    public interface IChatMessageRepository
    {
        Task SaveChatMessageAsync(ChatMessageDTO messageDTO);
        Task<IEnumerable<ChatMessageDTO>> GetAllChatMessagesByUserAsync(string userId);
    }
}
