using Azure.Storage.Queues;
using CityAlert.Entities;
using CityAlert.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CityAlert.Repositories
{
    public class LocationRepository : ILocationRepository
    {

        private CloudTableClient _tableClient;
        private CloudTable _locationsTable;
        private string _connectionString;
        private readonly string _tableName = "Location";
        private readonly string _queueName = "ins-upd-del-queue";


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



        public async Task<HttpResponseMessage> CreateLocation(Location location)
        {
            Guid GeneratedRowKey = Guid.NewGuid();
            Task<Location> locationTask = GetLocationById(location.PartitionKey, GeneratedRowKey.ToString());
            Location locationResult = locationTask.Result;
            if (locationResult == null)
            {
                location.RowKey = GeneratedRowKey.ToString();
                var insertOperation = TableOperation.Insert(location);

                await _locationsTable.ExecuteAsync(insertOperation);
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.OK, MyHttpResponse.SUCCESSFULL_OPERATION);
            }
            else
            {
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.NotFound, MyHttpResponse.ERROR_ITEM_EXISTENT);
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



        public async Task<HttpResponseMessage> DeleteLocation(string partitionKey, string rowKey)
        {
            Task<Location> locationTask =  GetLocationById(partitionKey, rowKey);
            Location location = locationTask.Result;
            if (location != null)
            {
                var deleteOperation = TableOperation.Delete(location);
                await _locationsTable.ExecuteAsync(deleteOperation);
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.OK, MyHttpResponse.SUCCESSFULL_OPERATION);
            }
            else
            {
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.NotFound, MyHttpResponse.ERROR_ITEM_NOT_FOUND);

            }
        }



        public async Task<HttpResponseMessage> UpdateLocation(Location location)
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
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.OK, MyHttpResponse.SUCCESSFULL_OPERATION);
            }
            else
            {
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.NotFound, MyHttpResponse.ERROR_ITEM_NOT_FOUND);
            }
        }



        public async Task<HttpResponseMessage> AddInQueueToCreate(Location location)
        {
            LocationOperation CreateOperation = new LocationOperation(location, LocationOperation.CREATE_OPERATION);

            var jsonLocation = JsonConvert.SerializeObject(CreateOperation);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonLocation);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(_connectionString, _queueName);
            await queueClient.SendMessageAsync(base64String);

            return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.OK, MyHttpResponse.SUCCESSFULL_OPERATION);
        }




        public async Task<HttpResponseMessage> AddInQueueToUpdate(Location location)
        {
            LocationOperation UpdateOperation = new LocationOperation(location, LocationOperation.UPDATE_OPERATION);

            var jsonLocation = JsonConvert.SerializeObject(UpdateOperation);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonLocation);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(_connectionString, _queueName);
            await queueClient.SendMessageAsync(base64String);

            return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.OK, MyHttpResponse.SUCCESSFULL_OPERATION);
        }



        public async Task<HttpResponseMessage> AddInQueueToDelete(string partitionKey, string rowKey)
        {
            LocationOperation DeleteOperation = new LocationOperation(new TableEntity(partitionKey, rowKey), LocationOperation.DELETE_OPERATION);

            var jsonLocation = JsonConvert.SerializeObject(DeleteOperation);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(jsonLocation);
            var base64String = System.Convert.ToBase64String(plainTextBytes);

            QueueClient queueClient = new QueueClient(_connectionString, _queueName);
            await queueClient.SendMessageAsync(base64String);

            return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.OK, MyHttpResponse.SUCCESSFULL_OPERATION);
        }
    }
}
