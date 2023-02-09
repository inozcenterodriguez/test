using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sat.Recruitment.Service.Dtos;
using Sat.Recruitment.Service.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    // Prueba Hecha por Marlon Calles, Software developer. 


    [ApiController]
    public class RecController : ControllerBase
    {

        private IUsersService usersService;
        private MyConfig myConfig;


        private readonly List<User> _users = new List<User>();

        public RecController(IUsersService _userService, IOptions<MyConfig> _options)
        {
            this.usersService = _userService;
            this.myConfig = _options.Value;
        }

        [HttpPost, Route("/Recruitment/users")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            await this.usersService.CreateUser(user, myConfig.UserTextPath);
            return Ok();
        }

        [HttpGet, Route("/Recruitment/users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await this.usersService.GetUsers(myConfig.UserTextPath);
            return Ok(users);
        }

    }
}
