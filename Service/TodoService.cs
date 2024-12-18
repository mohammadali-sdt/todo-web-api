using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeature;

namespace Service;

public class TodoService : ServiceBase<TodoDto>, ITodoService
{

    public TodoService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper) : base(repositoryManager, logger, mapper)
    {
    }

    public async Task<(IEnumerable<TodoDto> todos, MetaData metaData)> GetAllTodosAsync(Guid userId, TodoParameters todoParameters, bool trackChanges)
    {
        await CheckUserIsExists(userId, trackChanges);

        var todosWithMetaDate = await RepositoryManager.Todo.GetAllTodosAsync(userId, todoParameters, trackChanges);

        var todosDto = Mapper.Map<IEnumerable<TodoDto>>(todosWithMetaDate);

        return (todosDto, todosWithMetaDate.MetaData);

    }

    public async Task<TodoDto> GetTodoAsync(Guid userId, Guid todoId, bool trackChanges)
    {
        await CheckUserIsExists(userId, trackChanges);

        var todo = await CheckTodoIsExists(userId, todoId, trackChanges);

        var todoDto = Mapper.Map<TodoDto>(todo);

        return todoDto;
    }

    public async Task<TodoDto> CreateTodoAsync(TodoForCreationDto todo, Guid userId, bool trackChanges)
    {
        await CheckUserIsExists(userId, trackChanges);

        var todoEntity = Mapper.Map<Entities.Models.Todo>(todo);
        RepositoryManager.Todo.CreateTodo(userId, todoEntity);
        await RepositoryManager.SaveAsync();

        var todoDto = Mapper.Map<TodoDto>(todoEntity);

        return todoDto;
    }

    public async Task DeleteTodoAsync(Guid userId, Guid todoId, bool trackChanges)
    {
        await CheckUserIsExists(userId, trackChanges);

        var todoEntity = await CheckTodoIsExists(userId, todoId, trackChanges);

        RepositoryManager.Todo.DeleteTodo(todoEntity);
        await RepositoryManager.SaveAsync();
    }

    public async Task UpdateTodoAsync(TodoForUpdateDto todo, Guid userId, Guid todoId, bool userTrackChanges, bool todoTrackChanges)
    {
        await CheckUserIsExists(userId, userTrackChanges);

        var todoEntity = await CheckTodoIsExists(userId, todoId, todoTrackChanges);

        Mapper.Map(todo, todoEntity);

        await RepositoryManager.SaveAsync();
    }

    public async Task<(TodoForUpdateDto todoForUpdateDto, Entities.Models.Todo todoEntity)> PartiallyUpdateTodoAsync(Guid userId, Guid todoId, bool todoTrackChanges, bool userTrackChanges)
    {

        await CheckUserIsExists(userId, userTrackChanges);

        var todoEntity = await CheckTodoIsExists(userId, todoId, todoTrackChanges);

        var todoForPatch = Mapper.Map<TodoForUpdateDto>(todoEntity);

        return (todoForPatch, todoEntity);
    }

    public async Task SavePartiallyUpdateTodoAsync(TodoForUpdateDto todoForPatch, Entities.Models.Todo todoEntity)
    {
        Mapper.Map(todoForPatch, todoEntity);
        await RepositoryManager.SaveAsync();
    }

}