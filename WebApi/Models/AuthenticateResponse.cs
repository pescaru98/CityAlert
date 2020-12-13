using CityAlert.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Models
{
    public class AuthenticateResponse
    {
        public string RowKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            RowKey = user.RowKey;
            FirstName = user.FirstName;
            LastName = user.LastName;
            City = user.PartitionKey;
            Username = user.Username;
            Token = token;

        }
    }
}
