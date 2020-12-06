using L02.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L02.Repositories
{
    public class MetricsRepository : IMetricsRepository
    {

        private CloudTableClient _tableClient;
        private CloudTable _studentsTable;
        private string _connectionString;

        public MetricsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageConnectionString").ToString();
            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();
        }

        private async Task InitializeTable()
        {

            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _studentsTable = _tableClient.GetTableReference("metrics");

            await _studentsTable.CreateIfNotExistsAsync();
        }


        public async Task CreateMetrics(Metrics metrics)
        {
            var insertOperation = TableOperation.Insert(metrics);

            await _studentsTable.ExecuteAsync(insertOperation);
        }
    }
}
