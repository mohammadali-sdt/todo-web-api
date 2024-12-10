using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ITodoService
{
    IEnumerable<TodoDto> GetAllTodos(Guid userId, bool trackChanges);

    TodoDto GetTodo(Guid userId, Guid todoId, bool trackChanges);

    TodoDto CreateTodo(TodoForCreationDto todo, Guid userId, bool trackChanges);

    void DeleteTodo(Guid userId, Guid todoId, bool trackChanges);
}