using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;

namespace Service;

public abstract class ServiceBase: IServiceBase
{
    protected IRepositoryManager RepositoryManager { get; }
    protected ILoggerManager Logger { get; }
    protected IMapper Mapper { get; } 


    public ServiceBase(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
    {
        RepositoryManager = repositoryManager;
        Logger = logger;
        Mapper = mapper;
    }
    
    public async Task<Todo> CheckTodoIsExists(Guid userId, Guid todoId, bool trackChanges)
    {
        var todo = await RepositoryManager.Todo.GetTodoAsync(userId, todoId, trackChanges);

        if (todo == null) throw new TodoNotFoundException(todoId);

        return todo;
    }

    public async Task<User> CheckUserIsExists(Guid userId, bool trackChanges)
    {
        var user = await RepositoryManager.User.GetUserAsync(userId, trackChanges);

        if (user == null) throw new UserNotFoundException(userId);

        return user;
    }
}