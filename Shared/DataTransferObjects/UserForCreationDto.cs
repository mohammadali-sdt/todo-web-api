namespace Shared.DataTransferObjects;

public record UserForCreationDto
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public int Age { get; init; }
    public IEnumerable<TodoForCreationDto> Todos { get; init; }
}