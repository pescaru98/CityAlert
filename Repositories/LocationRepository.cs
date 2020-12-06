using CityAlert.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Repositories
{
    public class LocationRepository : ILocationRepository
    {

        private CloudTableClient _tableClient;
        private CloudTable _locationsTable;
        private string _connectionString;
        private string _tableName = "Location";

        public LocationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageConnectionString").ToString();
            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();
        }

        private async Task InitializeTable()
        {


            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _locationsTable = _tableClient.GetTableReference(_tableName);

            await _locationsTable.CreateIfNotExistsAsync();
        }

        public async Task CreateLocation(Location location)
        {
            
                var insertOperation = TableOperation.Insert(location);

                await _locationsTable.ExecuteAsync(insertOperation);
        }

        public async Task<List<Location>> GetLocationById(string partitionKey, string rowKey)
        {
            var locations = new List<Location>();

            TableQuery<Location> query = new TableQuery<Location>()
               .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToString()))
               .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToString()));
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<Location> resultSegment = await _locationsTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                locations.AddRange(resultSegment.Results);
            } while (token != null);

            return locations;
        }

        public async Task<List<Location>> GetLocations()
        {
            var locations = new List<Location>();

            TableQuery<Location> query = new TableQuery<Location>();
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<Location> resultSegment = await _locationsTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                locations.AddRange(resultSegment.Results);
            } while (token != null);

            return locations;
        }
    }
}
