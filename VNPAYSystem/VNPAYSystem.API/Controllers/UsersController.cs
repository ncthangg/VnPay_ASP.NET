using Microsoft.AspNetCore.Mvc;
using VNPAYSystem.Common.DTOs;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Service;

namespace VNPAYSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Login(LoginDto user)
        {

            var result = await _userService.Login(user.Email, user.Password);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }


    }
}
