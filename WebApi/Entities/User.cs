using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityAlert.Entities
{
    public class User : TableEntity
    {
        public User(string city, string rowKey)
        {
            this.PartitionKey = city;
            this.RowKey = rowKey;
        }

        public User()
        {

        }
        

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Username { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

    }
}
