using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Azure.Identity;

namespace CityAlert
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Task.Run(async () => { await Initialize(); })
                .GetAwaiter()
                .GetResult();
        }

        public static async Task Initialize()
        {
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                     var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                     config.AddAzureKeyVault(
                        keyVaultEndpoint,
                        new DefaultAzureCredential()
                     );
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                     webBuilder.UseStartup<Startup>();
                });
    }
}
