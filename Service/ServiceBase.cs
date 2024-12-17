using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;

namespace Service;

public abstract class ServiceBase<T> : IServiceBase
{
    protected IRepositoryManager RepositoryManager { get; }
    protected ILoggerManager Logger { get; }
    protected IMapper Mapper { get; }

    protected IDataShaper<T> DataShaper { get; }


    public ServiceBase(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IDataShaper<T> dataShaper)
    {
        RepositoryManager = repositoryManager;
        Logger = logger;
        Mapper = mapper;
        DataShaper = dataShaper;
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