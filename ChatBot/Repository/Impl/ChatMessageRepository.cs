using ChatBot.Data;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Repository.Impl
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly AppDbContext _context;

        public ChatMessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveChatMessageAsync(ChatMessageDTO messageDTO)
        {
            var chatMessage = new ChatMessage
            {
                UserId = messageDTO.UserId,
                Question = messageDTO.Question,
                PrettifiedResponse = messageDTO.PrettifiedResponse,
                Timestamp = messageDTO.Timestamp
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetAllChatMessagesByUserAsync(string userId)
        {
            return await _context.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .Select(m => new ChatMessageDTO
                {
                    UserId = m.UserId,
                    Question = m.Question,
                    PrettifiedResponse = m.PrettifiedResponse,
                    Timestamp = m.Timestamp
                })
                .ToListAsync();
        }
    }
}
