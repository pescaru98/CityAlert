using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L02.Entities;
using L02.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly ILogger<MetricsController> _logger;
        private IMetricsRepository _metricsRepository;

        public MetricsController(IMetricsRepository metricsRepository)
        {
            _metricsRepository = metricsRepository;
        }

        [HttpPost]
        public async Task CreateMetrics([FromBody] Metrics metrics)
        {
            try
            {
                await _metricsRepository.CreateMetrics(metrics);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
