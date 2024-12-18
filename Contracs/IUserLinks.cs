using Entities.LinkModels;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace Contracs;

public interface IUserLinks
{
    LinkResponse TryGenerateLinks(IEnumerable<UserDto> userDto, string fields, HttpContext httpContext);
}
