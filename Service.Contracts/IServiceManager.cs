namespace Service.Contracts;

public interface IServiceManager
{
    IUserService UserService { get; }
    ITodoService TodoService { get; }
    IAuthenticationService AuthenticationService { get; }
}