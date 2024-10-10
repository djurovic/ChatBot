using ChatBot.Models;
using ChatBot.Models.DTOs;

namespace ChatBot.Repository
{
    public interface IMicroserviceMethodRepository : IGenericRepository<MicroserviceMethod>
    {
        Task<IEnumerable<MicroserviceMethod>> GetMethodsByCatalogIdAsync(int catalogId);

    }
}
