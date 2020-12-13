using CityAlert.Entities;
using CityAlert.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Repositories
{
    public interface IUserRepository
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest request);

        Task<List<User>> GetAll();

        Task<User> GetByKeys(string partitionKey, string rowKey);

        Task<User> GetById(string rowKey);

    }
}
