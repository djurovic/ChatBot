using ChatBot.Data;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Repository.Impl
{
    public class MicroserviceMethodRepository : GenericRepository<MicroserviceMethod>, IMicroserviceMethodRepository
    {
        public MicroserviceMethodRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<MicroserviceMethod>> GetMethodsByCatalogIdAsync(int catalogId)
        {
            return await _context.MicroserviceMethods
                .Where(m => m.MicroserviceCatalogId == catalogId)
                .ToListAsync();
        }


    }
}
