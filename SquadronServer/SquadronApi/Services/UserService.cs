using Microsoft.EntityFrameworkCore;
using SquadronApi.Core;
using SquadronApi.Data;
using SquadronApi.Dto_s;
using SquadronApi.Services.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace SquadronApi.Services;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly TokenService _tokenService;

    public UserService(DataContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ServerResponse<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());

        if (user is null) return ServerResponse<UserDto>.Failure("User not found");

        if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            return ServerResponse<UserDto>.Failure("Wrong password");

        var userDto = new UserDto(user.Email, await _tokenService.CreateTokenAsync(user), user.DisplayName);

        return ServerResponse<UserDto>.Success(userDto);
    }

    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computeHash.SequenceEqual(passwordHash);
    }
}