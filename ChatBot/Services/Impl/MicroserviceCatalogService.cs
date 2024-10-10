using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Repository;

namespace ChatBot.Services.Impl
{
    public class MicroserviceCatalogService : IMicroserviceCatalogService
    {
        private readonly IMicroserviceCatalogRepository _catalogRepository;

        public MicroserviceCatalogService(IMicroserviceCatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }


        public async Task<MicroserviceCatalogDto> GetCatalogByIdAsync(int id)
        {
            var catalog = await _catalogRepository.GetByIdAsync(id);
            return MapToDto(catalog);
        }

        public async Task<IEnumerable<MicroserviceCatalogDto>> GetAllCatalogsAsync()
        {
            var catalogs = await _catalogRepository.GetAllAsync();
            return catalogs.Select(MapToDto);
        }

        public async Task<MicroserviceCatalogDto> CreateCatalogAsync(MicroserviceCatalogDto catalogDto)
        {
            var catalog = MapToEntity(catalogDto);
            await _catalogRepository.AddAsync(catalog);
            return MapToDto(catalog);
        }

        public async Task UpdateCatalogAsync(MicroserviceCatalogDto catalogDto)
        {
            var catalog = MapToEntity(catalogDto);
            await _catalogRepository.UpdateAsync(catalog);
        }

        public async Task DeleteCatalogAsync(int id)
        {
            await _catalogRepository.DeleteAsync(id);
        }

        public async Task<MicroserviceCatalogDto> GetCatalogWithMethodsAsync(int id)
        {
            return await _catalogRepository.GetCatalogWithMethodsAsync(id);
        }

        private MicroserviceCatalogDto MapToDto(MicroserviceCatalog catalog)
        {
            return new MicroserviceCatalogDto
            {
                Id = catalog.Id,
                Name = catalog.Name,
                MainLink = catalog.MainLink,
                Methods = catalog.Methods?.Select(m => new MicroserviceMethodDto
                {
                    Id = m.Id,
                    MethodName = m.MethodName,
                    MethodLink = m.MethodLink,
                    QuestionExample = m.QuestionExample,
                    DateInterpretationNeeded = m.DateInterpretationNeeded,
                    MicroserviceCatalogId = m.MicroserviceCatalogId
                }).ToList()
            };
        }

        private MicroserviceCatalog MapToEntity(MicroserviceCatalogDto catalogDto)
        {
            return new MicroserviceCatalog
            {
                Id = catalogDto.Id,
                Name = catalogDto.Name,
                MainLink = catalogDto.MainLink
            };
        }
    }
}
