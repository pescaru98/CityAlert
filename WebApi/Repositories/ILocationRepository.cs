using CityAlert.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CityAlert.Repositories
{
    public interface ILocationRepository
    {
        public Task<List<Location>> GetLocations();

        public Task<Location> GetLocationById(string partitionKey, string rowKey);

        public Task<HttpResponseMessage> CreateLocation(Location location);

        public Task<HttpResponseMessage> DeleteLocation(string partitionKey, string rowKey);

        public Task<HttpResponseMessage> UpdateLocation(Location location);

        public Task<Location> GetLocationByCoordinates(double latitude, double longitude);

        //public Task<List<Location>> GetLocationsByCoordinatesInRadius(double latitude, double longitude, double radiusInMeters);

        public Task<HttpResponseMessage> AddInQueueToCreate(Location location);

        public Task<HttpResponseMessage> AddInQueueToUpdate(Location location);

        public Task<HttpResponseMessage> AddInQueueToDelete(string partitionKey, string rowKey);
    }
}
