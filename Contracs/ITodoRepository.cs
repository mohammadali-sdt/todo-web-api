using Entities.Models;
using Shared.RequestFeature;

namespace Contracs;

public interface ITodoRepository
{
    Task<PagedList<Todo>> GetAllTodosAsync(Guid userId, TodoParameters todoParameters, bool trackChanges);
    Task<Todo> GetTodoAsync(Guid userId, Guid todoId, bool trackChanges);
    void CreateTodo(Guid userId, Todo todo);
    void DeleteTodo(Todo todo);
}
