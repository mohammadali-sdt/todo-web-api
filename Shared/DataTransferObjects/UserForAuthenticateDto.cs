using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class UserForAuthenticateDto
{
    [Required(ErrorMessage = "UserName or Email is a required field")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Password is a required field.")]
    [MaxLength(64, ErrorMessage = "Maximum length for the Password is 64 characters.")]
    [MinLength(8, ErrorMessage = "Minimum length for the Password is 8 characters.")]
    public string? Password { get; set; }
}
