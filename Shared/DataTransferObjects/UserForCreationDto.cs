using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record UserForCreationDto
{
    [Required(ErrorMessage = "User name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Name { get; init; }

    [Required(ErrorMessage = "Email is a required field.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? Email { get; init; }

    [Required(ErrorMessage = "Username is a required field")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Password is a required field.")]
    [MaxLength(64, ErrorMessage = "Maximum length for the Password is 64 characters.")]
    [MinLength(8, ErrorMessage = "Minimum length for the Password is 8 characters.")]
    public string? Password { get; init; }

    [Required(ErrorMessage = "Age is a required field.")]
    [Range(1, 100, ErrorMessage = "Age must be between {1} and {2}.")]
    public int Age { get; init; }

    public ICollection<string>? Roles { get; init; }
    public IEnumerable<TodoForCreationDto>? Todos { get; init; }
}