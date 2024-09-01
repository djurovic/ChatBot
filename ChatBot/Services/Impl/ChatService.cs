using ChatBot.Models;
using Newtonsoft.Json;
using System.Text;

namespace ChatBot.Services.Impl
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetChatResponse(string question)
        {
            string linkForMicroService = await GetLinkForMicroService(question);
            string responseFromMicroService = await GetResponseFromMicroService(linkForMicroService);
            string prettifyRes = await PrettifyResponse(responseFromMicroService);
            return prettifyRes;
        }

        private ChatRequest CreateChatRequest(string question)
        {
            
            return new ChatRequest
            {
                model = "llama3",
                messages = new[]
                {
                new Message { role = "user", content = $"{contextSentence} {question}" }
            },
                stream = false
            };
        }


        


        //Prvo "desifrujemo" koji mikroservis treba da se pozove
        private async Task<string> GetLinkForMicroService(string question)
        {
            var request = CreateChatRequest(question);
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/chat", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var chatResponse = JsonConvert.DeserializeObject<ChatResponse>(responseContent);
                return chatResponse.message.content;
            }

            throw new Exception("Failed to get response from the chat API");
        }

        //Drugo pozivamo odgovarajuci link
        private async Task<string> GetResponseFromMicroService(string link)
        {

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(link);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Optionally, you can deserialize the response here
                // var orders = JsonSerializer.Deserialize<Order[]>(responseBody);

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                return $"Request error: {e.Message}";
            }
        }

        //Trece radimo "prettify"
        private async Task<string> PrettifyResponse(string response1)
        {
            var request = CreatePrettifyRequest(response1);
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/chat", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var chatResponse = JsonConvert.DeserializeObject<ChatResponse>(responseContent);
                return chatResponse.message.content;
            }

            throw new Exception("Failed to get response from the chat API");
        }


        private ChatRequest CreatePrettifyRequest(string response)
        {
            
            return new ChatRequest
            {
                model = "llama3",
                messages = new[]
                {
                new Message { role = "user", content = $"{pretifySentence} {response}" }
            },
                stream = false
            };
        }


        
        private static string pretifySentence = "You will recive Json data that is a list of employees that we got from another microservice, the question was: Who was on vacation in May, you just need to prettify the reponse. You will recieve an array of json objects that have id, name, surname and vacations. Ignore that vacations are null. What i mean by pretify: RETURN JUST NAME AND SURNAME OF THE EMPLOYEES THAT IS VERY IMPORTANT THAT YOU JUST RETURN LIST OF NAMES AND SURNAMES.JUST RETURN LIST NOTHING ELSE!Your response should just be a list of names, nothing else nothing more than that. Your response SHOULDNT CONTAIN 'I can do that', 'Here is the prettfy reponse' and so on. It should just contain names and surnamse separated by comma";
        private static string contextSentence = "I have an microservice called Vacations, and it has a method http://localhost:6000/api/Vacation/by-date-interval?startDate={yyyy-mm-dd}&endDate={yyyy-mm-dd}, now your job is when i send you the message like 'who is in vaction in june' to respond with the link like this http://localhost:6000/api/Vacation/by-date-interval?startDate={yyyy-mm-dd}&endDate={yyyy-mm-dd},where startDate is 2024-06-01 and endDate is 2024-06-30,your answer HAS TO BE ONLY LINK THAT IS VERY IMPORTANT, so here is the question:";
    }
}
