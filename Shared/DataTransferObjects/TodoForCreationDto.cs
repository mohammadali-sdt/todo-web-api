namespace Shared.DataTransferObjects;

public record TodoForCreationDto
{
    public string? Title { get; init; }
        
    public string? Description { get; init; }
    
}