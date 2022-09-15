using System.Text;
using System.Text.Json;
using GeneralPurposeLib;

namespace AiChatbotWebApp.Utils; 

public static class AiManager {
    public static string Token { get; set; }
    
    public const string Personality = "A chatbot that is friendly\n\n";
    
    public static async Task<string> GetResponse(string prompt) {
        HttpClient client = new();
        client.BaseAddress = new Uri("https://api.openai.com/v1/completions");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
        string promptContent = prompt + "\nBot: ";
        StringContent content = new(
            new {
                model = "text-davinci-002",
                prompt = promptContent,
                temperature = 1,
                max_tokens = 100,
                top_p = 1,
                frequency_penalty = 2,
                presence_penalty = 2
            }.ToJson(), 
            Encoding.UTF8, 
            "application/json");
    
        HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);
        string responseString = await response.Content.ReadAsStringAsync();
        JsonDocument document = JsonDocument.Parse(responseString);
        JsonElement root = document.RootElement;
        JsonElement choices = root.GetProperty("choices");
        JsonElement choice = choices[0];
        JsonElement text = choice.GetProperty("text");
        return (text.GetString() ?? "").Replace("\n", "");
    }

}