using System.Dynamic;
using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeature;

namespace Service;

public class UserService : ServiceBase, IUserService
{

    private readonly IUserLinks _userLinks;
    private readonly UserManager<User> _userManager;

    public UserService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IUserLinks userLinks, UserManager<User> userManager) : base(repositoryManager, logger, mapper)
    {
        _userLinks = userLinks;
        _userManager = userManager;
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)> GetAllUsersAsync(LinkParameters linkParameters, bool trackChanges)
    {
        var users = await RepositoryManager.User.GetAllUsersAsync(linkParameters.userParameters, trackChanges);

        var usersDto = Mapper.Map<IEnumerable<UserDto>>(users);
        var links = _userLinks.TryGenerateLinks(userDto: usersDto, fields: linkParameters.userParameters.Fields ?? "", httpContext: linkParameters.HttpContext);

        return (linkResponse: links, users.MetaData);
    }

    public async Task<UserDto> GetUserAsync(Guid userId, bool trackChanges)
    {
        var user = await CheckUserIsExists(userId, trackChanges);

        var userDto = Mapper.Map<UserDto>(user);
        return userDto;
    }

    public async Task<UserDto> CreateUserAsync(UserForCreationDto user)
    {
        var userEntity = Mapper.Map<User>(user);

        // RepositoryManager.User.CreateUser(userEntity);
        // await RepositoryManager.SaveAsync();
        var result = await _userManager.CreateAsync(userEntity, user.Password);

        if (result.Succeeded && user.Roles != null)
        {
            await _userManager.AddToRolesAsync(userEntity, user.Roles);
        }

        var userToReturn = Mapper.Map<UserDto>(userEntity);

        return userToReturn;
    }

    public async Task<IEnumerable<UserDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new IdParametersBadRequestException();

        var userEntites = await RepositoryManager.User.GetByIdsAsync(ids, trackChanges);
        if (ids.Count() != userEntites.Count())
            throw new CollectionByIdsBadRequestException();

        var usersToReturn = Mapper.Map<IEnumerable<UserDto>>(userEntites);

        return usersToReturn;


    }

    public async Task<(IEnumerable<UserDto> users, string ids)> CreateUserCollectionAsync(IEnumerable<UserForCreationDto> userCollection)
    {
        if (userCollection is null) throw new UserCollectionBadRequest();

        var userEntities = Mapper.Map<IEnumerable<Entities.Models.User>>(userCollection);

        foreach (var user in userEntities)
        {
            RepositoryManager.User.CreateUser(user);
        }
        await RepositoryManager.SaveAsync();

        var userCollectionToReturn = Mapper.Map<IEnumerable<UserDto>>(userEntities);
        var ids = string.Join(",", userCollectionToReturn.Select(u => u.Id));


        return (userCollectionToReturn, ids);
    }

    public async Task DeleteUserAsync(Guid userId, bool trackChanges)
    {
        var user = await CheckUserIsExists(userId, trackChanges);

        RepositoryManager.User.DeleteUser(user);
        await RepositoryManager.SaveAsync();
    }

    public async Task UpdateUserAsync(UserForUpdateDto userForUpdateDto, Guid userId, bool trackChanges)
    {
        var userEntity = await CheckUserIsExists(userId, trackChanges);

        Mapper.Map(userForUpdateDto, userEntity);

        await RepositoryManager.SaveAsync();
    }

}