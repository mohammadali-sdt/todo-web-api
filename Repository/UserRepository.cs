using Contracs;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges) =>
        await FindAll(trackChanges).OrderBy(u => u.Name).ToListAsync();

    public async Task<User> GetUserAsync(Guid userId, bool trackChanges) =>
        await FindByCondition(u => u.Id.Equals(userId), trackChanges).SingleOrDefaultAsync();

    public void CreateUser(User user) => Create(user);
    
    public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) {
        return await FindByCondition(u => ids.Contains(u.Id), trackChanges).ToListAsync();
    }

    public void DeleteUser(User user) => Delete(user);
    
}
