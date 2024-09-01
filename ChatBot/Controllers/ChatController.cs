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


        [HttpGet("call-external-api")]
        public async Task<IActionResult> CallExternalApi()
        {
            string url = "http://localhost:6000/api/Vacation/by-date-interval?startDate=2024-01-01&endDate=2025-01-15";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Optionally, you can deserialize the response here
                // var orders = JsonSerializer.Deserialize<Order[]>(responseBody);

                return Ok(responseBody);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, $"Request error: {e.Message}");
            }
        }

        /*[HttpPost]
        public async Task<IActionResult> Chat()
        {
            string context = GetSentence();
            var request = new ChatRequest
            {
                model = "llama3",
                messages = new[]
                {
                new Message { role = "user", content = $"{context} Who is on vacation in May?" }
            },
                stream = false
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/chat", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var chatResponse = JsonConvert.DeserializeObject<ChatResponse>(responseContent);

                // Extract the content
                string extractedContent = chatResponse.message.content;

                return Ok(extractedContent);
            }
            else
            {
                return BadRequest("Failed to get response from the chat API");
            }
        }*/


        [HttpPost]
        public async Task<IActionResult> Chat(string question)
        {
            try
            {
                
                string response = await chatService.GetChatResponse(question);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }








    }
}

