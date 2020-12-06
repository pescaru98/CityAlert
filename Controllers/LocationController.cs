using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CityAlert.Entities;
using CityAlert.Repositories;

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

        [HttpGet]
        public async Task<IEnumerable<Location>> Get(){
            return await _locationRepository.GetLocations();
        }

        [HttpGet("{partitionKey}/{rowKey}")]
        public async Task<List<Location>> GetLocationById(string partitionKey, string rowKey)
        {
            return await _locationRepository.GetLocationById(partitionKey,rowKey);
        }

        [HttpPost]
        public async Task CreateLocation([FromBody] Location location)
        {
            try
            {
               await _locationRepository.CreateLocation(location);
            }catch(System.Exception)
            {
                throw;
            }
        }

/*        [HttpDelete("delete/{partitionKey}/{rowKey}")]
        public async Task DeleteLocation(string partitionKey, string rowKey)
        {
            try
            {
                await _locationRepository.DeleteLocation(partitionKey, rowKey);
            }
            catch (System.Exception)
            {
                throw;
            }
        }*/


    }
}
