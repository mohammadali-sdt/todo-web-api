using AutoMapper;
using Contracs;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;
namespace Service;


public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<ITodoService> _todoService;

    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IUserLinks userLinks, UserManager<User> userManager, IConfiguration configuration)
    {

        _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, logger, mapper, userLinks));

        _todoService = new Lazy<ITodoService>(() => new TodoService(repositoryManager, logger, mapper));

        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager, logger, mapper, userManager, configuration));
    }

    public IUserService UserService => _userService.Value;
    public ITodoService TodoService => _todoService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;

}