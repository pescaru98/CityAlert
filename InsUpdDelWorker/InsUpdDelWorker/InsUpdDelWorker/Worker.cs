using System;
using CityAlert.ControllerValidators;
using CityAlert.Repositories;
using InsUpdDelWorker.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InsUpdDelWorker
{
    
    public static class Worker
    {
        static LocationRepository _locationRepository;
        static string _connectionString;

        static Worker(){
            _connectionString = System.Environment.GetEnvironmentVariable("storageaccountcityab5a6_STORAGE");
            _locationRepository = new LocationRepository(_connectionString);
        }

        [FunctionName("Worker")]
        public async static void Run([QueueTrigger("ins-upd-del-queue", Connection = "storageaccountcityab5a6_STORAGE")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            LocationOperation locationOp = JsonConvert.DeserializeObject<LocationOperation>(myQueueItem);

            try
            {
                if (locationOp.OperationId == LocationOperation.CREATE_OPERATION)
                {
                    LocationValidator.validateCreateLocation(locationOp.location);
                    await _locationRepository.CreateLocation(locationOp.location);
                }
                else if (locationOp.OperationId == LocationOperation.UPDATE_OPERATION)
                {
                    LocationValidator.validateUpdateLocation(locationOp.location);
                    await _locationRepository.UpdateLocation(locationOp.location);
                }
                else if (locationOp.OperationId == LocationOperation.DELETE_OPERATION)
                {
                    LocationValidator.validateDeleteLocation(locationOp.location.PartitionKey, locationOp.location.RowKey);
                    await _locationRepository.DeleteLocation(locationOp.location.PartitionKey, locationOp.location.RowKey);
                }
            }
            catch (Exception)
            {
                //return notification
            }
        }
    }
}
