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
}
