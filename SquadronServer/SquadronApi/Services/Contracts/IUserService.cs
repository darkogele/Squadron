using SquadronApi.Core;
using SquadronApi.Dto_s;

namespace SquadronApi.Services.Contracts;

public interface IUserService
{
    Task<ServerResponse<UserDto>> Login(LoginDto loginDto);

    Task<ServerResponse<bool>> ChangePassword(ChangePasswordDto changePasswordDto);

    Task<ServerResponse<UserDto>> UpdateUser(UpdateUserDto updateUserDto);
}