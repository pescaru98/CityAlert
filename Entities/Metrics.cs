using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L02.Entities
{
    public class Metrics : TableEntity
    {
        public Metrics(string university, string uniqueKey)
        {
            this.PartitionKey = university;
            this.RowKey = uniqueKey;
        }

        public string University { get; }
        public string uniqueKey {get;}
        public int CountNumber { get; }
    }
   

}
