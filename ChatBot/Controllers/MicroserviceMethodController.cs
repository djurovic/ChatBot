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
    public class MicroserviceMethodController : ControllerBase
    {
        private readonly IMicroserviceMethodService _methodService;

        public MicroserviceMethodController(IMicroserviceMethodService methodService)
        {
            _methodService = methodService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MicroserviceMethod>>> GetAllMethods()
        {
            var methods = await _methodService.GetAllMethodsAsync();
            return Ok(methods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MicroserviceMethod>> GetMethod(int id)
        {
            var method = await _methodService.GetMethodByIdAsync(id);
            if (method == null)
            {
                return NotFound();
            }
            return Ok(method);
        }

        [HttpGet("catalog/{catalogId}")]
        public async Task<ActionResult<IEnumerable<MicroserviceMethod>>> GetMethodsByCatalogId(int catalogId)
        {
            var methods = await _methodService.GetMethodsByCatalogIdAsync(catalogId);
            return Ok(methods);
        }

        [HttpPost]
        public async Task<ActionResult<MicroserviceMethodDto>> CreateMethod(MicroserviceMethodDto methodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMethod = await _methodService.CreateMethodAsync(methodDto);

            var createdMethodDto = new MicroserviceMethodDto
            {
                Id = createdMethod.Id,
                MethodName = createdMethod.MethodName,
                MethodLink = createdMethod.MethodLink,
                QuestionExample = createdMethod.QuestionExample,
                DateInterpretationNeeded = createdMethod.DateInterpretationNeeded,
                MicroserviceCatalogId = createdMethod.MicroserviceCatalogId
            };

            return CreatedAtAction(nameof(GetMethod), new { id = createdMethodDto.Id }, createdMethodDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMethod(int id, MicroserviceMethodDto methodDto)
        {
            if (id != methodDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the DTO to the entity
            var method = new MicroserviceMethod
            {
                Id = methodDto.Id,
                MethodName = methodDto.MethodName,
                MethodLink = methodDto.MethodLink,
                QuestionExample = methodDto.QuestionExample,
                DateInterpretationNeeded = methodDto.DateInterpretationNeeded,
                MicroserviceCatalogId = methodDto.MicroserviceCatalogId
            };

            await _methodService.UpdateMethodAsync(method);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMethod(int id)
        {
            await _methodService.DeleteMethodAsync(id);
            return NoContent();
        }
    }
}
