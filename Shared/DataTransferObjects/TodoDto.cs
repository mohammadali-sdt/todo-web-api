namespace Shared.DataTransferObjects;


public record TodoDto
{
    public Guid Id { get; init; }
        
    public string? Title { get; init; }
        
    public string? Description { get; init; }
        
    public bool IsDone { get; init; }
}