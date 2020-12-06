using L02.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L02.Repositories
{
    public interface IMetricsRepository
    {
        public Task CreateMetrics(Metrics metrics);
    }
}
