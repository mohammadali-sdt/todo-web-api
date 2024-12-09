namespace Shared.DataTransferObjects;

public class UserForCreationDto
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public int Age { get; init; }
}