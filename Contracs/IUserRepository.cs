using Entities.Models;
using Shared.RequestFeature;

namespace Contracs;

public interface IUserRepository
{
    Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
    Task<User> GetUserAsync(Guid userId, bool trackChanges);
    void CreateUser(User user);

    Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    void DeleteUser(User user);
}
