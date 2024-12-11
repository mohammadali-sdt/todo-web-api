namespace Shared.DataTransferObjects;

public class TodoForUpdateDto
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    public bool IsDone { get; init; }
}