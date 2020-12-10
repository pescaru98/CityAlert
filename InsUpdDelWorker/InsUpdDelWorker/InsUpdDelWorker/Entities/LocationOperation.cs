using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsUpdDelWorker.Entities
{
    public class LocationOperation
    {
        public static readonly int CREATE_OPERATION = 1;
        public static readonly int UPDATE_OPERATION = 2;
        public static readonly int DELETE_OPERATION = 3;

        public LocationOperation(Location location, int operationId)
        {
            this.location = location;
            OperationId = operationId;
        }

        public Location location { get; set; }
        public int OperationId { get; set; }

    }
}
