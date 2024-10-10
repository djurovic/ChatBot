using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MicroserviceCatalogController : ControllerBase
    {
        private readonly IMicroserviceCatalogService _catalogService;

        public MicroserviceCatalogController(IMicroserviceCatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MicroserviceCatalogDto>>> GetAllCatalogs()
        {
            Console.WriteLine("Request recieved");
            var catalogs = await _catalogService.GetAllCatalogsAsync();
            return Ok(catalogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MicroserviceCatalogDto>> GetCatalog(int id)
        {
            var catalog = await _catalogService.GetCatalogByIdAsync(id);
            if (catalog == null)
            {
                return NotFound();
            }
            return Ok(catalog);
        }

        [HttpGet("{id}/methods")]
        public async Task<ActionResult<MicroserviceCatalogDto>> GetCatalogWithMethods(int id)
        {
            var catalog = await _catalogService.GetCatalogWithMethodsAsync(id);
            if (catalog == null)
            {
                return NotFound();
            }
            return Ok(catalog);
        }

        [HttpPost]
        public async Task<ActionResult<MicroserviceCatalogDto>> CreateCatalog(MicroserviceCatalogDto catalogDto)
        {
            var createdCatalog = await _catalogService.CreateCatalogAsync(catalogDto);
            return CreatedAtAction(nameof(GetCatalog), new { id = createdCatalog.Id }, createdCatalog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCatalog(int id, MicroserviceCatalogDto catalogDto)
        {
            if (id != catalogDto.Id)
            {
                return BadRequest();
            }

            await _catalogService.UpdateCatalogAsync(catalogDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalog(int id)
        {
            await _catalogService.DeleteCatalogAsync(id);
            return NoContent();
        }
    }
}
