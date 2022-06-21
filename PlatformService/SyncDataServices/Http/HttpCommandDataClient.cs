using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PlatformService.DTOs;

namespace platformservice.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        public HttpClient HttpClient { get; }
        public IConfiguration Configuration { get; }

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            Configuration = configuration;
            HttpClient = httpClient;
        }

        public async Task SendPlatformToCommand(PlatformReadDTO platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await HttpClient.PostAsync($"{Configuration["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("--> Sync POST to CommandService was OK");
            else
                Console.WriteLine("--> Sync POST to CommandService was NOT OK");
        }
    }
}