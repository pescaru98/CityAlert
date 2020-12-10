using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsUpdDelWorker.Entities
{
    public class Location : TableEntity
    {
        public Location(string city, string key)
        {
            this.PartitionKey = city;
            this.RowKey = key;
        }

        public Location(string city)
        {
            this.PartitionKey = city;
        }

        public Location()
        {

        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Comments { get; set; }
    }
}
