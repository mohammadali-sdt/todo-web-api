using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ITodoService
{
    IEnumerable<TodoDto> GetAllTodos(Guid userId, bool trackChanges);

    TodoDto GetTodo(Guid userId, Guid todoId, bool trackChanges);

    TodoDto CreateTodo(TodoForCreationDto todo, Guid userId, bool trackChanges);

    void DeleteTodo(Guid userId, Guid todoId, bool trackChanges);

    void UpdateTodo(TodoForUpdateDto todo, Guid userId, Guid todoId, bool userTrackChanges, bool todoTrackChanges);

    (TodoForUpdateDto todoForUpdateDto, Todo todoEntity) PartiallyUpdateTodo(Guid userId,
        Guid todoId, bool todoTrackChanges, bool userTrackChanges);

    void SavePartiallyUpdateTodo(TodoForUpdateDto todoForUpdateDto, Todo todoEntity);

}