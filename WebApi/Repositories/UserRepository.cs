using CityAlert.Entities;
using CityAlert.Models;
using CityAlert.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CityAlert.Repositories
{
    public class UserRepository : IUserRepository
    {
        private CloudTableClient _tableClient;
        private CloudTable _usersTable;
        private string _connectionString;
        private readonly string _tableName = "User";
        private readonly AppSettings _appSettings;

        public UserRepository(IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _connectionString = configuration.GetValue(typeof(string), "AzureStorageConnectionString").ToString();
            _appSettings = appSettings.Value;
            Task.Run(async () => { await InitializeTable(); }).GetAwaiter().GetResult();
        }



        private async Task InitializeTable()
        {


            var account = CloudStorageAccount.Parse(_connectionString);
            _tableClient = account.CreateCloudTableClient();

            _usersTable = _tableClient.GetTableReference(_tableName);

            await _usersTable.CreateIfNotExistsAsync();
        }


        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var users = new List<User>();

            TableQuery<User> query = new TableQuery<User>()
               .Where(TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, request.Username))
               .Where(TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, request.Password));
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<User> resultSegment = await _usersTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                users.AddRange(resultSegment.Results);
            } while (token != null);

            if (users == null)
                return null;
            var jwtToken = generateJwtToken(users.First());

            return new AuthenticateResponse(users.First(), jwtToken);

        }

        public async  Task<List<User>> GetAll()
        {
            var users = new List<User>();

            TableQuery<User> query = new TableQuery<User>();
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<User> resultSegment = await _usersTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                users.AddRange(resultSegment.Results);
            } while (token != null);

            return users;
        }

        public async Task<User> GetByKeys(string partitionKey, string rowKey)
        {
            var users = new List<User>();

            TableQuery<User> query = new TableQuery<User>()
               .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToString()))
               .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToString()));
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<User> resultSegment =  await _usersTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                users.AddRange(resultSegment.Results);
            } while (token != null);

            return users.FirstOrDefault();
        }

        public async Task<User> GetById(string rowKey)
        {
            var users = new List<User>();

            TableQuery<User> query = new TableQuery<User>()
               .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToString()));
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<User> resultSegment = await _usersTable.ExecuteQuerySegmentedAsync(query, token);

                token = resultSegment.ContinuationToken;

                users.AddRange(resultSegment.Results);
            } while (token != null);

            return users.FirstOrDefault();
        }



        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(new[] { new Claim("id", user.Username) }),
                Subject = new ClaimsIdentity(new[] { new Claim("rowKey", user.RowKey.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); //exception here
            return tokenHandler.WriteToken(token);
        }
    }
}
