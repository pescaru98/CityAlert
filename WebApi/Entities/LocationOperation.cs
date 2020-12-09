using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Entities
{
    public class LocationOperation
    {
        public static readonly int CREATE_OPERATION = 1;
        public static readonly int UPDATE_OPERATION = 2;
        public static readonly int DELETE_OPERATION = 3;

        public LocationOperation(ITableEntity location, int operationId)
        {
            this.location = location;
            OperationId = operationId;
        }

        public ITableEntity location { get; set; }
        public int OperationId { get; set; }

    }
}
