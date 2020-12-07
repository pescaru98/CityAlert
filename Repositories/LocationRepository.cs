﻿using CityAlert.Entities;
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
            Guid GeneratedRowKey = Guid.NewGuid();
            Task<Location> locationTask = GetLocationById(location.PartitionKey, GeneratedRowKey.ToString());
            Location locationResult = locationTask.Result;
            if (locationResult == null)
            {
                location.RowKey = GeneratedRowKey.ToString();
                var insertOperation = TableOperation.Insert(location);

                await _locationsTable.ExecuteAsync(insertOperation);
            }
            else
            {
                //Message: already existent
            }
        }

        public async Task<Location> GetLocationById(string partitionKey, string rowKey)
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

            return locations.FirstOrDefault();
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

        public async Task<Location> GetLocationByCoordinates(double latitude, double longitude)
        {
            var locations = new List<Location>();

            TableQuery<Location> query = new TableQuery<Location>()
               .Where(TableQuery.GenerateFilterConditionForDouble("Latitude", QueryComparisons.Equal, latitude))
               .Where(TableQuery.GenerateFilterConditionForDouble("Longitude", QueryComparisons.Equal, longitude));
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<Location> resultSegment = await _locationsTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                locations.AddRange(resultSegment.Results);
            } while (token != null);

            return locations.FirstOrDefault();
        }

/*        public async Task<List<Location>> GetLocationsByCoordinatesInRadius(double latitude, double longitude, double radiusInMeters)
        {
            var locations = new List<Location>();

            TableQuery<Location> query = new TableQuery<Location>()
               .Where(TableQuery.GenerateFilterConditionForDouble("Latitude", QueryComparisons.Equal, latitude))
               .Where(TableQuery.GenerateFilterConditionForDouble("Longitude", QueryComparisons.Equal, longitude));
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<Location> resultSegment = await _locationsTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                locations.AddRange(resultSegment.Results);
            } while (token != null);

            return locations;
        }*/

        public async Task DeleteLocation(string partitionKey, string rowKey)
        {
            Task<Location> locationTask =  GetLocationById(partitionKey, rowKey);
            Location location = locationTask.Result;
            if (location != null)
            {
                var deleteOperation = TableOperation.Delete(location);
                await _locationsTable.ExecuteAsync(deleteOperation);
            }
            else
            {
                //Message could not be found
            }
        }

        public async Task UpdateLocation(Location location)
        {
            Task<Location> locationTask = GetLocationById(location.PartitionKey, location.RowKey);
            Location locationResult = locationTask.Result;
            if (locationResult != null)
            {
                locationResult.Longitude = location.Longitude;
                locationResult.Latitude = location.Latitude;
                locationResult.Comments = location.Comments;

                var updateOperation = TableOperation.Replace(locationResult);
                await _locationsTable.ExecuteAsync(updateOperation);
            }
            else
            {
                //Message could not be found
            }
        }


    }
}
