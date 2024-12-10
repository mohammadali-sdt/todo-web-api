using Entities.Models;

namespace Contracs;

public interface ITodoRepository
{
    IEnumerable<Todo> GetAllTodos(Guid userId, bool trackChanges);
    Todo GetTodo(Guid userId, Guid todoId, bool trackChanges);
    void CreateTodo(Guid userId, Todo todo);
    void DeleteTodo(Todo todo);
}
