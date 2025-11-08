using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class LLMService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public LLMService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
    public AIApiResponse GetSuggestionsAsync(AIApiInput input)
    {

        AIApiResponse response = new AIApiResponse();
        try
        {
            Console.WriteLine(_configuration.ToString());

            string apiUrl = _configuration["OpenRouter:ApiUrl"];
            string apiKey = _configuration["OpenRouter:ApiKey"];
            string model = _configuration["OpenRouter:Model"];

            var requestBody = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "system", content = "You are an assistant that generates FAQs in JSON format [{question:'',answer:''}]." },

                    new { role = "user", content = $"Generate 3 FAQS related to: {input.prompt}" }
                }
            };
            var requestJson = JsonSerializer.Serialize(requestBody);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            httpRequest.Headers.Add("Authorization", $"Bearer {apiKey}");
            httpRequest.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            Console.WriteLine(requestJson);
            Console.WriteLine(httpRequest.Headers.ToString());
            var result = _httpClient.SendAsync(httpRequest).Result;
            if (!result.IsSuccessStatusCode)
            {
                response.isSuccess = false;
                response.errorText = $"Error from LLM API: {result.ReasonPhrase}";
                return response;

            }
            var doc = JsonDocument.Parse(result.Content.ReadAsStringAsync().Result);
            var suggestions = new List<AIApiResponse>();
            var choices = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            response.suggestions = JsonSerializer.Deserialize<List<AIResposneList>>(choices);
            response.isSuccess = true;
            
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.errorText = ex.Message;
        }
        return response;
    }

}