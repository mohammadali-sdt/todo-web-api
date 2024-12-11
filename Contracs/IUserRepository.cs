using Entities.Models;

namespace Contracs;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers(bool trackChanges);
    User GetUser(Guid userId, bool trackChanges);
    void CreateUser(User user);

    IEnumerable<User> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

    void DeleteUser(User user);
}
