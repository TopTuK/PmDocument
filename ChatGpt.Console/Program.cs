using NetChatGptCLient.Models.ChatGpt;
using NetChatGptCLient.Services.ChatGptClient;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

string apiKey = "fake api key";
string apiUrl = @"http://localhost:3000";
// var chatGptClient = ChatGptClientFactory.CreateDefaultClient(apiKey);
var chatGptClient = ChatGptClientFactory.CreateApiClient(apiUrl, apiKey);

string? promt;

Console.Write("Enter assistant value: ");
promt = Console.ReadLine();

if ((promt == null) || (promt == string.Empty))
{
    return;
}

var conversation = await chatGptClient.StartNewConversation(promt);

while(true)
{
    Console.Write("User: ");
    promt = Console.ReadLine();

    if (promt is not { Length: > 0})
    {
        break;
    }

    var message = await chatGptClient.AskAsync(conversation, promt);
    Console.WriteLine($"Assistant (ChatGPT): {message}");
}