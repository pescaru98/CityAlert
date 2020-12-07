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

        public Task<Location> GetLocationById(string partitionKey, string rowKey);

        public Task CreateLocation(Location location);

        public Task DeleteLocation(string partitionKey, string rowKey);

        public Task UpdateLocation(Location location);

        public Task<Location> GetLocationByCoordinates(double latitude, double longitude);

        //public Task<List<Location>> GetLocationsByCoordinatesInRadius(double latitude, double longitude, double radiusInMeters);
    }
}
