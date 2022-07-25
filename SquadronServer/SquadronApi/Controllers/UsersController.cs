using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquadronApi.Dto_s;
using SquadronApi.Services.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace SquadronApi.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Login"), AllowAnonymous]
    [SwaggerOperation(Summary = "Login User")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        return HandleResult(await _userService.Login(loginDto));
    }

    [HttpPost("register"), AllowAnonymous]
    [SwaggerOperation(Summary = "Register User")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        return HandleResult(await _userService.Register(registerDto));
    }

    [HttpPost("Change-Password")]
    [SwaggerOperation(Summary = "Change user password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        return HandleResult(await _userService.ChangePassword(changePasswordDto));
    }

    [HttpPut("Edit-user")]
    [SwaggerOperation(Summary = "Self edit user data")]
    public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
    {
        return HandleResult(await _userService.UpdateUser(updateUserDto));
    }
}