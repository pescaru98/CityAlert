using System;
using CityAlert.Repositories;
using InsUpdDelWorker.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InsUpdDelWorker
{
    [StorageAccount("cityalertstorage_STORAGE")]
    public static class Worker
    {
        static LocationRepository _locationRepository;
        static string _connectionString;

        static Worker(){
            _connectionString = System.Environment.GetEnvironmentVariable("cityalertstorage_STORAGE");
            _locationRepository = new LocationRepository(_connectionString);
        }

        [FunctionName("Worker")]
        public async static void Run([QueueTrigger("ins-upd-del-queue", Connection = "cityalertstorage_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            LocationOperation locationOp = JsonConvert.DeserializeObject<LocationOperation>(myQueueItem);

            if (locationOp.OperationId == LocationOperation.CREATE_OPERATION)
                await _locationRepository.CreateLocation(locationOp.location);
            else if (locationOp.OperationId == LocationOperation.UPDATE_OPERATION)
                await _locationRepository.UpdateLocation(locationOp.location);
            else if (locationOp.OperationId == LocationOperation.DELETE_OPERATION)
                await _locationRepository.DeleteLocation(locationOp.location.PartitionKey, locationOp.location.RowKey);
        }
    }
}
