using System.Dynamic;
using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeature;

namespace Service.Contracts;

public interface IUserService
{
    Task<(LinkResponse linkResponse, MetaData metaData)> GetAllUsersAsync(LinkParameters linkParameters, bool trackChanges);
    Task<UserDto> GetUserAsync(Guid userId, bool trackChanges);
    Task<UserDto> CreateUserAsync(UserForCreationDto user);

    Task<IEnumerable<UserDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task<(IEnumerable<UserDto> users, string ids)> CreateUserCollectionAsync(IEnumerable<UserForCreationDto> userCollection);

    Task DeleteUserAsync(Guid userId, bool trackChanges);

    Task UpdateUserAsync(UserForUpdateDto userForUpdateDto, Guid userId, bool trackChanges);
}