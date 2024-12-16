using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeature;

namespace Service.Contracts;

public interface ITodoService
{
    Task<(IEnumerable<TodoDto> todos, MetaData metaData)> GetAllTodosAsync(Guid userId, TodoParameters todoParameters ,bool trackChanges);

    Task<TodoDto> GetTodoAsync(Guid userId, Guid todoId, bool trackChanges);

    Task<TodoDto> CreateTodoAsync(TodoForCreationDto todo, Guid userId, bool trackChanges);

    Task DeleteTodoAsync(Guid userId, Guid todoId, bool trackChanges);

    Task UpdateTodoAsync(TodoForUpdateDto todo, Guid userId, Guid todoId, bool userTrackChanges, bool todoTrackChanges);

    Task<(TodoForUpdateDto todoForUpdateDto, Todo todoEntity)> PartiallyUpdateTodoAsync(Guid userId,
        Guid todoId, bool todoTrackChanges, bool userTrackChanges);

    Task SavePartiallyUpdateTodoAsync(TodoForUpdateDto todoForUpdateDto, Todo todoEntity);

}