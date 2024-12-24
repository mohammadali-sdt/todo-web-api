using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record class UserForRegisterationDto
{
    [Required(ErrorMessage = "Username is a required field")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email is a required field.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is a required field.")]
    [MaxLength(64, ErrorMessage = "Maximum length for the Password is 64 characters.")]
    [MinLength(8, ErrorMessage = "Minimum length for the Password is 8 characters.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "PasswordConfirm is a required field.")]
    [MaxLength(64, ErrorMessage = "Maximum length for the PasswordConfirm is 64 characters.")]
    [MinLength(8, ErrorMessage = "Minimum length for the PasswordConfirm is 8 characters.")]
    public string? PasswordConfirm { get; set; }

    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Name { get; set; }

    [Range(1, 100, ErrorMessage = "Age must be between {1} and {2}.")]
    public int Age { get; set; }
}
