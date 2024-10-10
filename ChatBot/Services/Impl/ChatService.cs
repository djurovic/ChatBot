using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Repository;
using Newtonsoft.Json;
using System.Text;

namespace ChatBot.Services.Impl
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatService(HttpClient httpClient, IChatMessageRepository chatMessageRepository)
        {
            _httpClient = httpClient;
            _chatMessageRepository = chatMessageRepository;
        }
        public async Task<string> GetChatResponse(string userId, string question)
        {
            string linkForMicroService = await GetLinkForMicroService(question);
            string responseFromMicroService = await GetResponseFromMicroService(linkForMicroService);
            string prettifyRes = await PrettifyResponse(responseFromMicroService, question, linkForMicroService);
            await SaveQuestionAndAnswer(userId, question, prettifyRes);
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
        private async Task<string> PrettifyResponse(string response1, string question, string linkForMicroService)
        {
            var request = CreatePrettifyRequest(response1, question, linkForMicroService);
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


        private ChatRequest CreatePrettifyRequest(string response, string question, string link)
        {
            //{userQuestion}{ linkFromLama}
           

            return new ChatRequest
            {
                model = "llama3",
                messages = new[]
                {
                new Message { role = "user", content = $"{pretifySentence} {question} {link} {response}" }
            },
                stream = false
            };
        }

        private async Task SaveQuestionAndAnswer(string userId, string question, string prettifiedResponse)
        {
            var chatMessageDTO = new ChatMessageDTO
            {
                UserId = userId,
                Question = question,
                PrettifiedResponse = prettifiedResponse,
                Timestamp = DateTime.UtcNow
            };

            await _chatMessageRepository.SaveChatMessageAsync(chatMessageDTO);
        }



        //private static string pretifySentence = "You will recive Json data that is a list of employees that we got from another microservice, the question was: Who was on vacation in May, you just need to prettify the reponse. You will recieve an array of json objects that have id, name, surname and vacations. Ignore that vacations are null. What i mean by pretify: RETURN JUST NAME AND SURNAME OF THE EMPLOYEES THAT IS VERY IMPORTANT THAT YOU JUST RETURN LIST OF NAMES AND SURNAMES.JUST RETURN LIST NOTHING ELSE!Your response should just be a list of names, nothing else nothing more than that. Your response SHOULDNT CONTAIN 'I can do that', 'Here is the prettfy reponse' and so on. It should just contain names and surnamse separated by comma";
        //private static string contextSentence = "I have an microservice called Vacations, and it has a method http://localhost:6000/api/Vacation/by-date-interval?startDate={yyyy-mm-dd}&endDate={yyyy-mm-dd}, now your job is when i send you the message like 'who is in vaction in june' to respond with the link like this http://localhost:6000/api/Vacation/by-date-interval?startDate={yyyy-mm-dd}&endDate={yyyy-mm-dd},where startDate is 2024-06-01 and endDate is 2024-06-30,your answer HAS TO BE ONLY LINK THAT IS VERY IMPORTANT, so here is the question:";
        private static string contextSentence = $"Date Interpretation:- 'Yesterday' refers to the day before the current date.- 'Last [day of week]' refers to the most recent past occurrence of that day. 'Last week' refers to the 7-day period ending last Sunday.- Today's date is {DateTime.UtcNow}. Use this as reference for relative dates." +
            @"I have three microservices:CheckIn, Orders and Vacations. Your task is to receive user question and repspond with the correct link to microservice. Here are the microservices with links and responses



Check in Microservice
Main link: http://localhost:6002/api/Employee/
Api methods:

Method: GetWhoIsInNow() that returns list of employees that are currently in the office
Link: http://localhost:6002/api/Employee/who-is-in-now
Example :'Who is in the office now?' and similar  to respond with the link 'http://localhost:6002/api/Employee/who-is-now'


Method: GetWhoIsOutNow() that returns list of employees that are currently out of the office
Link: http://localhost:6002/api/Employee/who-is-out-now
Example: 'Who isn't at the office right now?' and similar respond with the link 'http://localhost:6002/api/Employee/who-is-out-now'


Method: IsEmployeeInOrOut([FromQuery] string employeeName, [FromQuery] stirng employeeSurname) that checks if given employee is in the office.Method returns In or Out
Link: http://localhost:6002/api/Employee/is-in-or-out?employeeName={employeeName}&employeeSurname={employeeSurname}
Example: 'Is Donna in the office?' 'Is John in?' and similar respond with the link 'http://localhost:6002/api/Employee/http://localhost:6002/api/Employee/is-in-or-out?employeeName={employeeName} where {employeeName} is the name from the question




Orders Microservice
Main link: http://localhost:6004/api/Restaurant
Api methods:
Method: GetRestaurantsByDate([FromQuery] DateTime date) that returns list of restaurants that were ordered on the given date
Link:http://localhost:6004/api/Restaurant/by-date?date={date}
Example:'Where did we order yesterday?' 'Where did we ordered last tuesday' and similar and you respond with the link 'http://localhost:6004/api/Restaurant/by-date?date={date}' where {date] is appropriate date from the question
meaning if it's for example yesterday you take the date for yesterday and include in the link. Example today is 09-05-2024, and if the user ask what did we order yesterday you set date to 09-04-2024
Date Interpretation:- 'Yesterday' refers to the day before the current date.- 'Last [day of week]' refers to the most recent past occurrence of that day. 'Last week' refers to the 7-day period ending last Sunday.'Next month' refers to next month from todays date.- Today's date is: " + DateTime.UtcNow.ToString("d MMMM yyyy") + @" Use this as reference for relative dates.


Method:GetRestaurantsByDateInterval([FromQuery] DateTime startDate, [FromQuery] DateTime endDate) that returns list of restaurant for the given time interval
Link:http://localhost:6004/api/Restaurant/by-date-interval?startDate={date}&endDate={date}
Example:'From where did we order last week?' and you respond with the link http://localhost:6004/api/Restaurant/by-date-interval?startDate={date}&endDate={date}
Date Interpretation:- 'Yesterday' refers to the day before the current date.- 'Last [day of week]' refers to the most recent past occurrence of that day. 'Last week' refers to the 7-day period ending last Sunday.'Next month' refers to next month from todays date.- Today's date is: " + DateTime.UtcNow.ToString("d MMMM yyyy") + @" Use this as reference for relative dates.

Methods: GetOrdersByRestaurantName([FromQuery] string restaurantName) that returns list of orders for the given restaurant
Link: http://localhost:6004/api/Restaurant/orders?restaurantName={restaurantName}
Example: 'When did we order from 'that restauran' ?' and similar you respond with link 'http://localhost:6004/api/Restaurant/orders?restaurantName={restaurantName}' and {restaurantName} is from the question



//Vacation
        Main link:http://localhost:6000/api/Vacation/ 
Api method:

Method: GetEmployeesonVacationbyDateInterval([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
Link: http://localhost:6000/api/Vacation/ by-date-interval?startDate={yyyy-mm-dd}&endDate={yyyy-mm-dd}
Example: 'Who is in vaction in june' to respond with the link like this 'http://localhost:6000/api/Vacation/by-date-interval?startDate={yyyy-mm-dd}&endDate={yyyy-mm-dd}', where startDate is 2024-06-01 and endDate is 2024-06-30
Date Interpretation:- 'Yesterday' refers to the day before the current date.- 'Last [day of week]' refers to the most recent past occurrence of that day. 'Last week' refers to the 7-day period ending last Sunday.'Next month' refers to next month from todays date.- Today's date is:  " + DateTime.UtcNow.ToString("d MMMM yyyy") + @" Use this as reference for relative dates.

Method: GetVacationByEmployee(string firstName, string lastName)
Link: http://localhost:6000/api/Vacation/by-employee?firstName={firstName}&lastName={lastName}
Example: 'When is Dana Bins on vacation?' you respond with the link like this 'http://localhost:5111/api/Vacation/by-employee?firstName=dana&lastName=bins'

IF YOU CANT FIGURE OUT THE LINK RESPOND WITH 'Can you repeat the question'
IF YOU CAN DETERMINE WHAT MICROSERVICE SHOULD BE CALLED RESPOND ONLY WITH THE LINK
YOUR ANSWER HAS TO BE ONLY LINK THAT IS VERY IMPORTANT, so here is the question:";


        private static string pretifySentence = @"Your job is to pretify Json data that you will recieve. Json data can come from three microservices. CheckIn microservice, Order microservice and Vacation microservice. These are the posible json objects that you will have to preitfy,

Check In Microservice
Methods:

Method name:GetWhoIsInNow(). It returns array of Employes that are currently in the office
Link: http://localhost:6002/api/Employee/who-is-now
This is the type of object you will recieve in response :[
{
'id': 1,
'name': 'Estrella'
'surname': 'Jenkins',
'checkIns': null
}
]
If array from api is empty you will recieve empty array like this:[]
If response is empty it will be: 'No employee is currently in the office.'
Your response should look like this ''Here are the employees that are currently in: {name surname}, {name surname}, ...
You can ignore 'id' and 'checkIns'
If response is empty your response should be 'No employee is currently in the office.'

Method name: GetWhoIsOutNow() It return array of Employees that aren't in the office currentyl
Link: http://localhost:6002/api/Employee/who-is-out-now
This is the type of object you will recieve in response:
{
'id': 1,
'name': 'Estrella',
'surname': 'Jenkins',
'checkIns': null
}
If array from api is empty you will recieve empty array like this:[]
If reponse is empty: 'Everybody is in the office'
Your response should look like this: 'Here are the employees that are currently out of the office: {name surname}, {name surname}, ...
You can ignore 'id' and 'checkIns'
if response is empty return 'All employees are in the office right now.'

Method name: IsEmployeeInOrOut([FromQuery] string employeeName) returns 'In or out'
Link: http://localhost:6002/api/Employee/is-in-or-out?employeeName={employeeName}
Type of respnse: In or Out
If response is empty: $'No employee found with the name '{employeeName}' or no check-ins recorded.'
Your response should look like this. '{employeeName} is In/Out' depending of the api response
If response is something else besides In or Out you will return: 'No employee found or no chek-ins recorded'


Order Microservice
Methods:

Method name: GetRestaurantsByDate([FromQuery] DateTime date) it returns arrays of restaurants that were ordered from on given date
Link:http://localhost:6004/api/Restaurant/by-date?date={date}
Type of object you will recieve:[
{
'id': 8,
'name': 'Toast Burgers',
'orders': null
},
{
 'id': 20,
'name': 'Ginos pizza',
'orders': null
}
]
If response is empty you will recieve: 'No orders were made for {date}' or empty array '[]'
Your response should look like this 'On {date} it was ordered from these restaurants: {restaurantName}, {restaurantName},..'
You can ignore 'id' and 'orders'
If response is empty return ''No orders were made for {date}'

Method name: GetRestaurantsByDateInterval([FromQuery] DateTime startDate, [FromQuery] DateTime endDate) it returns array of restaurants that were ordered from for given time interval
Link:http://localhost:6004/api/Restaurant/by-date-interval?startDate={date}&endDate={date}
Type of object you will recieve:
[
{
'id': 8,
'name': 'Le petit pigeon',
'orders': null
},
{
    'id': 20,
'name': 'A la Dino',
'orders': null
}
]
If response is empty: 'No orders were made in interval from {startDate} to {endDate}' or you will recieve  empty array '[]'
Your response should look like this: 'Between {startDate} and {endDate} orders were made from this restaurant: {restaurantName, date?}, {restaurantName}..
You can ignore 'id' and 'orders'
If response is empty return 'No orders were made in interval from {startDate} to {endDate}'


Method name: GetOrdersByRestaurant(string restaurantName)
Link: http://localhost:6004/api/Restaurant/orders?restaurantName={restaurantName}
Type of object you will recieve:
[
{
'id': 3,
'dateOrdered': '2024-01-02T00:00:00',
'restaurantId': 8,
'restaurant': null
},
{
    'id': 28,
'dateOrdered': '2024-01-18T00:00:00',
'restaurantId': 8,
'restaurant': null
},
{
    'id': 43,
'dateOrdered': '2024-01-30T00:00:00',
'restaurantId': 8,
'restaurant': null
}
If response is empty you will recieve 'No orders found for restaurant '{restaurantName}' or empty array '[]'
If response is empty: 'No orders found for restaurant '{ restaurantName}
'
Your response should look like this 'For {restaurntName} orders were made on {orderDate}, {orderDate}...' and you will transform date from 'yyyy-mm-ddThh;mm:ss' to 'dd/mm/yyyy'
You can ignore 'id', 'restaurantId' and 'restaurant'
If response is empty return 'No orders found for restaurant '{restaurantName}'


Vacation Microservice
Methods:

Method name: GetEmployeesOnVacationByDateInterval(DateTime startDate, DateTime endDate) it returns list of employees that are on vacation for given interval
Link: http://localhost:6000/api/Vacation/by-date-interval?startDate={yyyy-mm-dd}&endDate={yyyy-mm-dd}
Type of object you will recieve:
[
{
'id': 2,
'name': 'Ebba',
'surname': 'Zboncak',
'vacations: null
},
{
'id': 17,
'name': 'Winfield',
'surname': 'Gleason',
'vacations': null
}
]
If response is empty: $'No employee is on vacation between {startDate} {endDate}' or empty array '[]'
Your response should look like this 'Employees that are on vaction from {startDate} to {endDate} are {firstName lastName}, {firstName lastName}'..
You can ignore 'id' and 'vacations'
If response is empty return 'No employee is on vacation between {startDate} {endDate}'

Method name: GetVacationByEmployee(string firstName, string lastName) that returns list of vacations for given employee
Link: http://localhost:6000/api/Vacation/by-employee?firstName={firstName}&lastName={lastName}
Type of object it will recieve:
[
{
'id': 91,
'startDate': '2024-01-26T00:00:00',
'endDate': '2024-02-05T00:00:00',
'employeeId': 46,
'employee': null
},
{
 'id': 92,
'startDate': '2024-06-26T00:00:00'
'endDate': '2024-07-05T00:00:00',
'employeeId': 46,
'employee': null
}
]
If response is empty:'No vacations found for {firstName} {lastName}' or empty array '[]'
Your response should look like this '{firstName lastName} is on vaction for these dates: {startDate endDate}, {startDate endDate}...' and you will transform date from 'yyyy-mm-ddThh:mm:ss' to 'dd/mm/yyyy'
If response is empty return 'No vacations found for {firstName} {lastName}'


What i mean by pretify: Return it so it look like i wrote you it should look

Now, here is the userQuestion, link and response, just return based on what i wrote you. dont include anything more.Your response MUSTN'T CONTAIN 'I can do that', 'Here is the prettfy reponse' and so on.";
    }
}
