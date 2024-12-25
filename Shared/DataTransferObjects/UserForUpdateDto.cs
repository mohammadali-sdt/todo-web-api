namespace Shared.DataTransferObjects;

public class UserForUpdateDto
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? UserName { get; init; }
    public int Age { get; init; }
    public IEnumerable<TodoForCreationDto>? Todos { get; init; }
}