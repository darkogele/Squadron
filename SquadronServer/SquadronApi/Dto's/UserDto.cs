using System.ComponentModel.DataAnnotations;

namespace SquadronApi.Dto_s;

public record UserDto(string Email, string Token, string DisplayName);

public record LoginDto([EmailAddress] string Email, [StringLength(100, MinimumLength = 5)] string Password);

public record UpdateUserDto([EmailAddress] string Email, [StringLength(30, MinimumLength = 3)] string DisplayName);