using ChatBot.Data;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Repository.Impl
{
    public class MicroserviceCatalogRepository : GenericRepository<MicroserviceCatalog>, IMicroserviceCatalogRepository
    {
        public MicroserviceCatalogRepository(AppDbContext context) : base(context) { }

        public async Task<MicroserviceCatalogDto> GetCatalogWithMethodsAsync(int id)
        {
            var catalog = await _context.MicroserviceCatalogs
                .Include(c => c.Methods)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (catalog == null)
                return null;

            return new MicroserviceCatalogDto
            {
                Id = catalog.Id,
                Name = catalog.Name,
                MainLink = catalog.MainLink,
                Methods = catalog.Methods.Select(m => new MicroserviceMethodDto
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
    }
}
