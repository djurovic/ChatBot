using ChatBot.Models;
using ChatBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace ChatBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IChatService chatService;

        public ChatController(HttpClient httpClient, IChatService chatService)
        {
            _httpClient = httpClient;
            this.chatService = chatService;
        }


        [HttpGet("secretTest")]
        public IActionResult Get()
        {
            return Ok("This is a secret message for authenticated users only");
        }

        


        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequestModel model)
        {
            try
            {
                string response = await chatService.GetChatResponse(model.UserId, model.Question);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

    public class ChatRequestModel
    {
        public string UserId { get; set; }
        public string Question { get; set; }
    }

}

