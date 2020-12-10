using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CityAlert.Entities;
using CityAlert.Repositories;
using System.Net.Http;
using CityAlert.ControllerValidators;
using CityAlert.Utils;

namespace CityAlert.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    { 
        private readonly ILogger<LocationController> _logger;
        private ILocationRepository _locationRepository;
        /*        public LocationController(ILogger<LocationController> logger)
                {        

                    _logger = logger;
                }*/

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<Location>> Get(){
            return await _locationRepository.GetLocations();
        }

        [HttpGet("GetById/{partitionKey}/{rowKey}")]
        public async Task<Location> GetLocationById(string partitionKey, string rowKey)
        {
            return await _locationRepository.GetLocationById(partitionKey,rowKey);
        }

        [HttpPost("Create")]
        public async Task<HttpResponseMessage> CreateLocation([FromBody] Location location)
        {
            try
            {
                LocationControllerValidator.validateCreateLocation(location);
               return await _locationRepository.CreateLocation(location);
            }catch(System.Exception)
            {
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.BadRequest,MyHttpResponse.ERROR_INVALID);
                //throw;
            }
        }

        [HttpDelete("Delete/{partitionKey}/{rowKey}")]
        public async Task<HttpResponseMessage> DeleteLocation(string partitionKey, string rowKey)
        {
            try
            {
                LocationControllerValidator.validateDeleteLocation(partitionKey,rowKey);
                return await _locationRepository.DeleteLocation(partitionKey, rowKey);
            }
            catch (System.Exception)
            {
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.BadRequest, MyHttpResponse.ERROR_INVALID);
                //throw;
            }
        }

        [HttpPut("Update")]
        public async Task<HttpResponseMessage> UpdateLocation([FromBody] Location location)
        {
            try
            {
                LocationControllerValidator.validateUpdateLocation(location);
                return await _locationRepository.UpdateLocation(location);
            }
            catch (System.Exception)
            {
                return MyHttpResponse.CreateResponse(System.Net.HttpStatusCode.BadRequest, MyHttpResponse.ERROR_INVALID);
                //throw;
            }
        }

        [HttpGet("GetByCoordinates/{latitude}/{longitude}")]
        public async Task<Location> GetByCoordinates(double latitude, double longitude)
        {

              return await _locationRepository.GetLocationByCoordinates(latitude,longitude);

        }

/*        [HttpGet("GetByCoordinates/{latitude}/{longitude}/{radius}")]
        public async Task<List<Location>> GetByCoordinatesInRadius(double latitude, double longitude,double radiusInMeters)
        {
             return await _locationRepository.GetLocationsByCoordinatesInRadius(latitude, longitude, radiusInMeters);
        }*/

        [HttpPost("AddInQueueToCreate")]
        public async Task<HttpResponseMessage> AddInQueueToCreate([FromBody] Location location)
        {
            try
            {
                return await _locationRepository.AddInQueueToCreate(location);
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        [HttpPut("AddInQueueToUpdate")]
        public async Task<HttpResponseMessage> AddInQueueToUpdate([FromBody] Location location)
        {
            try
            {
                return await _locationRepository.AddInQueueToUpdate(location);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpDelete("AddInQueueToDelete/{partitionKey}/{rowKey}")]
        public async Task<HttpResponseMessage> AddInQueueToDelete(string partitionKey, string rowKey)
        {
            try
            {
                return await _locationRepository.AddInQueueToDelete(partitionKey, rowKey);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
