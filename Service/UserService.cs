using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
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

    public UserDto CreateUser(UserForCreationDto user)
    {
        var userEntity = _mapper.Map<User>(user);
        
        _repositoryManager.User.CreateUser(userEntity);
        _repositoryManager.Save();

        var userToReturn = _mapper.Map<UserDto>(userEntity);

        return userToReturn;
    }

    public IEnumerable<UserDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var userEntites = _repositoryManager.User.GetByIds(ids, trackChanges);
        if (ids.Count() != userEntites.Count())
            throw new CollectionByIdsBadRequestException();

        var usersToReturn = _mapper.Map<IEnumerable<UserDto>>(userEntites);

        return usersToReturn;


    }

    public (IEnumerable<UserDto> users, string ids) CreateUserCollection(IEnumerable<UserForCreationDto> userCollection)
    {
        if (userCollection is null) throw new UserCollectionBadRequest();

        var userEntities = _mapper.Map<IEnumerable<User>>(userCollection);

        foreach (var user in userEntities)
        {
            _repositoryManager.User.CreateUser(user);
        }
        _repositoryManager.Save();

        var userCollectionToReturn = _mapper.Map<IEnumerable<UserDto>>(userEntities);
        var ids = string.Join(",", userCollectionToReturn.Select(u => u.Id));
        
        
        return(userCollectionToReturn, ids);
    }

    public void DeleteUser(Guid userId, bool trackChanges)
    {
        var user = _repositoryManager.User.GetUser(userId, trackChanges);

        if (user is null) throw new UserNotFoundException(userId);
        
        _repositoryManager.User.DeleteUser(user);
        _repositoryManager.Save();
    }

    public void UpdateUser(UserForUpdateDto userForUpdateDto, Guid userId, bool trackChanges)
    {
        var userEntity = _repositoryManager.User.GetUser(userId, trackChanges);

        if (userEntity is null) throw new UserNotFoundException(userId);

        _mapper.Map(userForUpdateDto, userEntity);
        
        _repositoryManager.Save();
    }
    
}