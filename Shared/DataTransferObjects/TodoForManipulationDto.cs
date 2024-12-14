using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record TodoForManipulationDto
{
    [Required(ErrorMessage = "Title is a required field.")]
    [MaxLength(120, ErrorMessage = "Maximum length of Title is 120 characters.")]
    public string? Title { get; init; }
    
    [MaxLength(500, ErrorMessage = "Maximum length of Description is 500 characters.")]
    public string? Description { get; init; }
};
