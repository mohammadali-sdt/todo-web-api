using Contracs;
using Entities.Models;

namespace Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public IEnumerable<User> GetAllUsers(bool trackChanges) =>
        FindAll(trackChanges).OrderBy(u => u.Name).ToList();

    public User GetUser(Guid userId, bool trackChanges) =>
        FindByCondition(u => u.Id.Equals(userId), trackChanges).SingleOrDefault();

    public void CreateUser(User user) => Create(user);
    
    public IEnumerable<User> GetByIds(IEnumerable<Guid> ids, bool trackChanges) {
        return FindByCondition(u => ids.Contains(u.Id), trackChanges).ToList();
    }

    public void DeleteUser(User user) => Delete(user);
    
}
