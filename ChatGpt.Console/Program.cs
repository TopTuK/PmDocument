using ChatGpt.Console;
using NetChatGptCLient.Services.ChatGptClient;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

string apiKey = "";
var chatGptClient = ChatGptClientFactory.CreateDefaultClient(apiKey);

var conversation = chatGptClient.StartNewConversation("Answer me only wrong answers");
await chatGptClient.AskAsync(conversation, "Calculate 2+2*2");