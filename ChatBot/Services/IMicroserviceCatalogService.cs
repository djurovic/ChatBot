using ChatBot.Models.DTOs;

namespace ChatBot.Services
{
    public interface IMicroserviceCatalogService
    {
        Task<MicroserviceCatalogDto> GetCatalogByIdAsync(int id);
        Task<IEnumerable<MicroserviceCatalogDto>> GetAllCatalogsAsync();
        Task<MicroserviceCatalogDto> CreateCatalogAsync(MicroserviceCatalogDto catalogDto);
        Task UpdateCatalogAsync(MicroserviceCatalogDto catalogDto);
        Task DeleteCatalogAsync(int id);
        Task<MicroserviceCatalogDto> GetCatalogWithMethodsAsync(int id);
    }
}
