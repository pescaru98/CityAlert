using CityAlert.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Repositories
{
    public interface ILocationRepository
    {
        public Task<List<Location>> GetLocations();

        public Task<List<Location>> GetLocationById(string partitionKey, string rowKey);

        public Task CreateLocation(Location location);

        //public Task DeleteLocation(string partitionKey, string rowKey);
    }
}
