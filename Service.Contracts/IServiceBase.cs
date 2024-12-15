using Entities.Models;

namespace Service.Contracts;

public interface IServiceBase
{
    Task<Todo> CheckTodoIsExists(Guid userId, Guid todoId, bool trackChanges);
    Task<User> CheckUserIsExists(Guid userId, bool trackChanges);
}