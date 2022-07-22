namespace SquadronApi.Dto_s;

public record UserDto(string Email, string Token, string DisplayName);

public record LoginDto(string Email, string Password);
