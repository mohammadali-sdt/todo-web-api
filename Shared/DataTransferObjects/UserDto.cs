namespace Shared.DataTransferObjects;

public record UserDto(Guid Id, string Name, string Email, string Username, int Age)
{
    
}