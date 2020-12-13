using CityAlert.Entities;
using CityAlert.Models;
using CityAlert.Repositories;
using CityAlert.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityAlert.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;


        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("Auth")]
        public IActionResult Auth(AuthenticateRequest request)
        {
            var response = _userRepository.Authenticate(request);

            if (response.Result == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(response.Result);
        }

        [Authorize]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = _userRepository.GetAll();

            return Ok(users.Result);
        }
 
    }
}
