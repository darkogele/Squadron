using System.ComponentModel.DataAnnotations;

namespace SquadronApi.Dto_s;

//public class ChangePasswordDto(
//    [StringLength(100, MinimumLength = 5)] string CurrentPassword,
//    [StringLength(100, MinimumLength = 5)] string NewPassword,
//    [Compare("NewPassword", ErrorMessage = "The new Password and Confirmation password do not match.")] string ConfirmNewPassword);

public class ChangePasswordDto
{
    [Required]
    [StringLength(30, MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required]
    [Compare("NewPassword", ErrorMessage = "The new Password and Confirmation password do not match.")]
    public string ConfirmNewPassword { get; set; }
}