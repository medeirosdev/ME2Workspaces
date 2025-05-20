using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using OpenAI.Chat;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ME2Workspaces.Components.Pages.Me2IA
{
    public class ME2IAService
    {
        private const string API_KEY = "OG1oRFBKM2liaUxKSkJUTmJ0NU5YTVZZN0tpS2RMQ250Ulp3clQ2cFNEMEUyZ2I5Qm5sMEpRUUo5OUJFQUNab3lmaVhKM3czQUFBQkFDT0c3a3RB"; // Set your key here

        public static async Task<string> PerguntarIA(string pergunta)
        {
            try
            {

           
            var endpoint = new Uri("https://openaime2.openai.azure.com/");
            var deploymentName = "o3-mini";
            var apiKey = DecodeBase64(API_KEY);

            AzureOpenAIClient azureClient = new(
                endpoint,
                new AzureKeyCredential(apiKey));
            ChatClient chatClient = azureClient.GetChatClient(deploymentName);


            List<ChatMessage> messages = new List<ChatMessage>()
                {
                    new SystemChatMessage(pergunta),
                    new UserChatMessage("Responda a pergunta acima com detalhe e com muita inteligência."),
                };

            var response = chatClient.CompleteChat(messages);

            return response.Value.Content[0].Text;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                return "Erro";  
            }
        }

        public static string DecodeBase64(string base64Encoded)
        {
            if (string.IsNullOrWhiteSpace(base64Encoded))
                return string.Empty;

            // Remove eventuais espaços ou quebras de linha
            base64Encoded = base64Encoded.Trim();

            try
            {
                byte[] bytes = Convert.FromBase64String(base64Encoded);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                // Lançado se a string não for Base64 válida
                throw new ArgumentException("Entrada não é uma string Base64 válida.", nameof(base64Encoded));
            }
        }
    }
}
