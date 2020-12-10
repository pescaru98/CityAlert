using InsUpdDelWorker.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CityAlert.Repositories
{
    public interface ILocationRepository
    {
        public Task<Location> GetLocationById(string partitionKey, string rowKey);

        public Task CreateLocation(Location location);

        public Task DeleteLocation(string partitionKey, string rowKey);

        public Task UpdateLocation(Location location);

    }
}
