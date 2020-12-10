﻿using CityAlert.Exceptions;
using InsUpdDelWorker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.ControllerValidators
{
    public class LocationValidator
    {
        public static void validateCreateLocation(Location location)
        {
            if (location.PartitionKey == null)
                throw new MissingKeyException();
        }

        public static void validateDeleteLocation(string partitionKey, string rowKey)
        {
            if(partitionKey == null || rowKey == null) 
                throw new MissingKeyException();
        }

        public static void validateUpdateLocation(Location location)
        {
            if(location.RowKey == null || location.PartitionKey == null)
                throw new MissingKeyException();
        }

    }
}
