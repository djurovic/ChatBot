using ChatBot.Models;
using ChatBot.Models.DTOs;

namespace ChatBot.Services
{
    public interface IMicroserviceMethodService
    {
        Task<MicroserviceMethod> GetMethodByIdAsync(int id);
        Task<IEnumerable<MicroserviceMethod>> GetAllMethodsAsync();
        Task<MicroserviceMethod> CreateMethodAsync(MicroserviceMethodDto methodDto);
        Task UpdateMethodAsync(MicroserviceMethod method);
        Task DeleteMethodAsync(int id);
        Task<IEnumerable<MicroserviceMethod>> GetMethodsByCatalogIdAsync(int catalogId);
    }
}
