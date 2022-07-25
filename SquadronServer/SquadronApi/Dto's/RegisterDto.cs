using System.ComponentModel.DataAnnotations;

namespace SquadronApi.Dto_s;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(30, MinimumLength = 3)]
    public string DisplayName { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 5)]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

}