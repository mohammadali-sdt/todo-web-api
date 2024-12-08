using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Todo
{
    [Column("TodoId")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is a required field.")]
    [MaxLength(120, ErrorMessage = "Maximum length of Title is 120 characters.")]
    public string? Title { get; set; }

    [MaxLength(500, ErrorMessage = "Maximum length of Description is 500 characters.")]
    public string? Description { get; set; }

    public bool IsDone { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public User? User { get; set; }

}
