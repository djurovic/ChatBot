using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Repository;

namespace ChatBot.Services.Impl
{
    public class MicroserviceMethodService : IMicroserviceMethodService
    {
        private readonly IMicroserviceMethodRepository _methodRepository;

        public MicroserviceMethodService(IMicroserviceMethodRepository methodRepository)
        {
            _methodRepository = methodRepository;
        }

        public async Task<MicroserviceMethod> GetMethodByIdAsync(int id)
        {
            return await _methodRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<MicroserviceMethod>> GetAllMethodsAsync()
        {
            return await _methodRepository.GetAllAsync();
        }

        public async Task<MicroserviceMethod> CreateMethodAsync(MicroserviceMethodDto methodDto)
        {
            var method = new MicroserviceMethod
            {
                MethodName = methodDto.MethodName,
                MethodLink = methodDto.MethodLink,
                QuestionExample = methodDto.QuestionExample,
                DateInterpretationNeeded = methodDto.DateInterpretationNeeded,
                MicroserviceCatalogId = methodDto.MicroserviceCatalogId
            };

            await _methodRepository.AddAsync(method);

            return method;
        }

        public async Task UpdateMethodAsync(MicroserviceMethod method)
        {
            await _methodRepository.UpdateAsync(method);
        }

        public async Task DeleteMethodAsync(int id)
        {
            await _methodRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<MicroserviceMethod>> GetMethodsByCatalogIdAsync(int catalogId)
        {
            return await _methodRepository.GetMethodsByCatalogIdAsync(catalogId);
        }

        
    }
}
