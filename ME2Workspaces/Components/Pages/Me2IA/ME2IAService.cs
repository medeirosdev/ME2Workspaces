﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ME2Workspaces.Components.Pages.Me2IA
{
    public class ME2IAService
    {
        private const string API_KEY = ""; // Set your key here

        private const string ENDPOINT = "https://ai-guilhermeellena7680ai336718873135.openai.azure.com/openai/deployments/gpt-4o-mini/chat/completions?api-version=2024-02-15-preview";

        public static async Task<string> PerguntarIA(string pergunta)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("api-key", API_KEY);
                var payload = new
                {
                    messages = new object[]
                    {
                  new {
                      role = "system",
                      content = new object[] {
                          new {
                              type = "text",
                              text = "You are an AI assistant that helps people find information."
                          }
                      }
                  },
                  new {
                      role = "user",
                      content = new object[] {
                          new {
                              type = "image_url",
                              image_url = new {
                                  url = $"data:image/jpeg;base64"
                              }
                          },
                          new {
                              type = "text",
                              text = pergunta
                          }
                      }
                  }
                    },
                    temperature = 0.7,
                    top_p = 0.95,
                    max_tokens = 800,
                    stream = false
                };

                var response = await httpClient.PostAsync(ENDPOINT, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var responseData = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                    return responseData;
                }
                else
                {
                    return $"Error: {response.StatusCode}, {response.ReasonPhrase}";
                }
            }
        }

    }
}
