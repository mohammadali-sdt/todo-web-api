using Contracs;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeature;

namespace Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public async Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges)
    {
        var users = await FindByCondition(u => u.Age >= userParameters.MinAge && u.Age <= userParameters.MaxAge, trackChanges)
            .OrderBy(u => u.Name)
            .Skip((userParameters.PageNumber - 1) * userParameters.PageSize)
            .Take(userParameters.PageSize)
            .ToListAsync();

        var count = await FindByCondition(u => u.Age >= userParameters.MinAge && u.Age <= userParameters.MaxAge, trackChanges).CountAsync();

        return new PagedList<User>(users, count, userParameters.PageNumber, userParameters.PageSize);
    }

    public async Task<User> GetUserAsync(Guid userId, bool trackChanges) =>
        await FindByCondition(u => u.Id.Equals(userId), trackChanges).SingleOrDefaultAsync();

    public void CreateUser(User user) => Create(user);
    
    public async Task<IEnumerable<User>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) {
        return await FindByCondition(u => ids.Contains(u.Id), trackChanges).ToListAsync();
    }

    public void DeleteUser(User user) => Delete(user);
    
}
