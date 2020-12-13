using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Entities
{
    //Possible improvement: add city and key as class attributes, add them DataAnnotations so you eliminate the others validators
    //or add [Validator] annotation to validate them
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

        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        //[Required]
        [MaxLength(1000)]
        public string Comments { get; set; }
    }
}
