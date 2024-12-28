using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models;
public class User : IdentityUser<Guid>
{

    [Column("UserId")]
    public override Guid Id { get; set; }

    [Required(ErrorMessage = "Username is a required field")]
    public override string? UserName { get; set; }

    [Required(ErrorMessage = "Email is a required field.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public override string? Email { get; set; }

    [Required(ErrorMessage = "Password is a required field.")]
    [MaxLength(64, ErrorMessage = "Maximum length for the Password is 64 characters.")]
    [MinLength(8, ErrorMessage = "Minimum length for the Password is 8 characters.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "User name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Age is a required field.")]
    [Range(1, 100, ErrorMessage = "Age must be between {1} and {2}.")]
    public int Age { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }

    public ICollection<Todo>? Todos { get; set; }
}
