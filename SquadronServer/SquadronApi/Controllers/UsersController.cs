using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquadronApi.Dto_s;
using SquadronApi.Services.Contracts;

namespace SquadronApi.Controllers
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await _userService.Login(loginDto);

            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}