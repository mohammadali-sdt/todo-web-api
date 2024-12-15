using Entities.Models;

namespace Contracs;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllTodosAsync(Guid userId, bool trackChanges);
    Task<Todo> GetTodoAsync(Guid userId, Guid todoId, bool trackChanges);
    void CreateTodo(Guid userId, Todo todo);
    void DeleteTodo(Todo todo);
}
