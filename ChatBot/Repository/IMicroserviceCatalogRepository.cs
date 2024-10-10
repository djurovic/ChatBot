using ChatBot.Models;
using ChatBot.Models.DTOs;

namespace ChatBot.Repository
{
    public interface IMicroserviceCatalogRepository : IGenericRepository<MicroserviceCatalog>
    {
        Task<MicroserviceCatalogDto> GetCatalogWithMethodsAsync(int id);
    }
}
