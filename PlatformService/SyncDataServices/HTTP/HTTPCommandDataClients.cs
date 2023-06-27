using Microsoft.Extensions.Configuration;
using PlatformService.Models.DTOs;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.HTTP
{
    public class HTTPCommandDataClients : ICommandDataClient
    {

        public readonly HttpClient _httpClient1;
        public readonly IConfiguration _configuration;
        public HTTPCommandDataClients(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient1 = httpClient;  
            _configuration = configuration;
        }

        public async Task  SendPlatformToCommand(PlatformDTO platform)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(platform),Encoding.UTF8,"application/json");
            var response = await _httpClient1.PostAsync("http://localhost:6000/api/commands/Platforms", httpContent);
            if (response.IsSuccessStatusCode)
            {
               Console.WriteLine("---> SYNC POST TO COMMANDSERVICE WAS OKAY");
            }
           
        }



    }
}
