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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(DataContext context, TokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServerResponse<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());

        if (user is null) return ServerResponse<UserDto>.Failure("User not found");

        if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            return ServerResponse<UserDto>.Failure("Wrong password");

        var userDto = new UserDto(
            user.Email,
            await _tokenService.CreateTokenAsync(user),
            user.DisplayName);

        return ServerResponse<UserDto>.Success(userDto);
    }

    public async Task<ServerResponse<bool>> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var user = await _context.Users.FindAsync(userId);

        if (user is null)
            return ServerResponse<bool>.Failure("User not found in Database");

        if (!VerifyPasswordHash(changePasswordDto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            return ServerResponse<bool>.Failure("Old password is incorrect");

        CreatePasswordHash(changePasswordDto.NewPassword, out var passwordHash, out var passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        var result = await _context.SaveChangesAsync() > 0;

        return result
            ? ServerResponse<bool>.Success(true)
            : ServerResponse<bool>.Failure($"Failed to update Password for user: {user.DisplayName} in database");
    }

    public async Task<ServerResponse<UserDto>> UpdateUser(UpdateUserDto updateUserDto)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var user = await _context.Users.FindAsync(userId);

        if (user is null)
            return ServerResponse<UserDto>.Failure("User not found in Database");

        user.Email = updateUserDto.Email;
        user.DisplayName = updateUserDto.DisplayName;

        var result = await _context.SaveChangesAsync() > 0;

        return result
            ? ServerResponse<UserDto>.Success(
                new UserDto(user.Email,
                            await _tokenService.CreateTokenAsync(user),
                            user.DisplayName))
            : ServerResponse<UserDto>.Failure($"Failed to update the user: {updateUserDto.DisplayName} in database");
    }

    public async Task<ServerResponse<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Email))
            return ServerResponse<UserDto>.Failure("Email is already taken");

        CreatePasswordHash(registerDto.Password, out var passwordHash, out var passwordSalt);

        var userForDb = new Entities.User
        {
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _context.Users.AddAsync(userForDb);

        var result = await _context.SaveChangesAsync() > 0;

        return result
           ? ServerResponse<UserDto>.Success(
                           new UserDto(userForDb.Email,
                           await _tokenService.CreateTokenAsync(userForDb),
                           userForDb.DisplayName))
           : ServerResponse<UserDto>.Failure($"Failed to update the user: {userForDb.DisplayName} in database");
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computeHash.SequenceEqual(passwordHash);
    }

    private async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}