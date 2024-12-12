using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class TodoService : ITodoService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;


    public TodoService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<TodoDto> GetAllTodos(Guid userId, bool trackChanges)
    {
        var user = _repositoryManager.User.GetUser(userId, trackChanges);

        if (user == null) throw new UserNotFoundException(userId);

        var todos = _repositoryManager.Todo.GetAllTodos(userId, trackChanges);

        var todosDto = _mapper.Map<IEnumerable<TodoDto>>(todos);

        return todosDto;

    }

    public TodoDto GetTodo(Guid userId, Guid todoId, bool trackChanges)
    {
        var user = _repositoryManager.User.GetUser(userId, trackChanges);

        if (user == null) throw new UserNotFoundException(userId);

        var todo = _repositoryManager.Todo.GetTodo(userId, todoId, trackChanges);

        if (todo == null) throw new TodoNotFoundException(todoId);

        var todoDto = _mapper.Map<TodoDto>(todo);

        return todoDto;
    }

    public TodoDto CreateTodo(TodoForCreationDto todo, Guid userId, bool trackChanges)
    {
        var user = _repositoryManager.User.GetUser(userId, trackChanges);

        if (user is null) throw new UserNotFoundException(userId);
        
        var todoEntity = _mapper.Map<Todo>(todo);
        _repositoryManager.Todo.CreateTodo(userId, todoEntity);
        _repositoryManager.Save();

        var todoDto = _mapper.Map<TodoDto>(todoEntity);

        return todoDto;
    }

    public void DeleteTodo(Guid userId, Guid todoId, bool trackChanges)
    {
        var userEntity = _repositoryManager.User.GetUser(userId, trackChanges);

        if (userEntity is null) throw new UserNotFoundException(userId);
            
        var todoEntity = _repositoryManager.Todo.GetTodo(userId, todoId, trackChanges);

        if (todoEntity is null) throw new TodoNotFoundException(todoId);
        
        _repositoryManager.Todo.DeleteTodo(todoEntity);
        _repositoryManager.Save();
    }

    public void UpdateTodo(TodoForUpdateDto todo, Guid userId, Guid todoId, bool userTrackChanges, bool todoTrackChanges)
    {
        var userEntity = _repositoryManager.User.GetUser(userId, userTrackChanges);

        if (userEntity is null) throw new UserNotFoundException(userId);

        var todoEntity = _repositoryManager.Todo.GetTodo(userId, todoId, todoTrackChanges);

        if (todoEntity is null) throw new TodoNotFoundException(todoId);
        
        _mapper.Map(todo, todoEntity);
        
        _repositoryManager.Save();
    }

    public (TodoForUpdateDto todoForUpdateDto, Todo todoEntity) PartiallyUpdateTodo(Guid userId, Guid todoId, bool todoTrackChanges, bool userTrackChanges)
    {

        var user = _repositoryManager.User.GetUser(userId, userTrackChanges);

        if (user is null) throw new UserNotFoundException(userId);

        var todoEntity = _repositoryManager.Todo.GetTodo(userId, todoId, todoTrackChanges);

        if (todoEntity is null) throw new TodoNotFoundException(todoId);

        var todoForPatch = _mapper.Map<TodoForUpdateDto>(todoEntity);

        return (todoForPatch, todoEntity);
    }

    public void SavePartiallyUpdateTodo(TodoForUpdateDto todoForPatch, Todo todoEntity)
    {
        _mapper.Map(todoForPatch, todoEntity);
        _repositoryManager.Save();
    }

}