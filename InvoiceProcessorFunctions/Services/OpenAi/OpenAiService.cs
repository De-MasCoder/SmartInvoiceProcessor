using InvoiceProcessor.Contracts.Models;
using OpenAI.Chat;
using System.Text.Json;

namespace InvoiceProcessorFunctions.Services.OpenAi
{
    public class OpenAiService : IOpenAiService
    {
        private readonly ChatClient _client;

        public OpenAiService(ChatClient chatClient)
        {
            _client = chatClient;
        }

        public async Task<InvoiceData?> ExtractAsync(string content)
        {
            var response = await _client.CompleteChatAsync(
                new ChatMessage[]
                {
            ChatMessage.CreateSystemMessage(
                "Extract invoice data as STRICT JSON with fields: vendor, amount, date. No explanation, no extra text."
            ),
            ChatMessage.CreateUserMessage(content)
                });

            var json = response.Value.Content[0].Text;

            try
            {
                return JsonSerializer.Deserialize<InvoiceData>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch
            {
                return null; // fallback if AI gives bad JSON
            }
        }
    }
}
