namespace Shared.DataTransferObjects;


public record UserDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Username { get; init; }
    public int Age { get; init; }
}