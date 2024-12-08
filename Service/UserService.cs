using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;


    public UserService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<UserDto> GetAllUsers(bool trackChanges)
    {
        var users = _repositoryManager.User.GetAllUsers(trackChanges);

        // var usersDto = users.Select(u =>
        //     new UserDto(
        //         Username: u.Username,
        //         Email: u.Email,
        //         Id: u.Id,
        //         FullName: u.Name,
        //         Age:  u.Age
        //         )).ToList();

        var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

        return usersDto;
    }

    public UserDto GetUser(Guid userId, bool trackChanges)
    {
        var user = _repositoryManager.User.GetUser(userId, trackChanges);

        if (user == null) throw new UserNotFoundException(userId);
        
        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

}