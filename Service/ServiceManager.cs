using AutoMapper;
using Contracs;
using Service.Contracts;
using Shared.DataTransferObjects;
namespace Service;


public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<ITodoService> _todoService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IUserLinks userLinks)
    {

        _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, logger, mapper, userLinks));

        _todoService = new Lazy<ITodoService>(() => new TodoService(repositoryManager, logger, mapper));
    }

    public IUserService UserService => _userService.Value;
    public ITodoService TodoService => _todoService.Value;

}