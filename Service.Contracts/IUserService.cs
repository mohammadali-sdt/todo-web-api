using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IUserService
{
    IEnumerable<UserDto> GetAllUsers(bool trackChanges);
    UserDto GetUser(Guid userId, bool trackChanges);
}