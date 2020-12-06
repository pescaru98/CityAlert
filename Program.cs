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

namespace L02
{
    public class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Task.Run(async () => { await Initialize(); })
                .GetAwaiter()
                .GetResult();
        }

        public static async Task Initialize()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;" +
                "AccountName=storagepesca2020;" +
                "AccountKey=OYRJ6Iud7ET+CUUcnUL69YB4bs9H9Mpm5Pd2y9t1kYHOlS44FeZ2l6iDfsIOr3Hypf++j62mWH1ulx8ENz5FDw==;" +
                "EndpointSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("studenti");

            await studentsTable.CreateIfNotExistsAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
