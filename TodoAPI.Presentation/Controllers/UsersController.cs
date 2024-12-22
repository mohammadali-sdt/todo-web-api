using System.Text.Json;
using Asp.Versioning;
using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeature;
using TodoAPI.Presentation.ActionFilters;
using TodoAPI.Presentation.ModelBinder;

namespace TodoAPI.Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/users")]
[ApiController]
// [ResponseCache(CacheProfileName = "120SecondsDuration")]
[OutputCache(PolicyName = "120SecondsDurationPolicy")]
public class UsersController : ControllerBase
{
    private readonly IServiceManager _service;

    public UsersController(IServiceManager service)
    {
        _service = service;
    }

    [HttpOptions]
    public IActionResult GetUsersOptions()
    {
        Response.Headers["Allow"] = "GET, POST, PUT, PATCH, DELETE, OPTIONS";
        return Ok();
    }

    [HttpGet(Name = "GetUsers")]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    // [ResponseCache(Duration = 60)]
    [OutputCache(Duration = 20)]
    public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParameters)
    {
        var linkParms = new LinkParameters(userParameters, HttpContext);
        var result = await _service.UserService.GetAllUsersAsync(linkParms, false);

        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.metaData);

        return result.linkResponse.HasLinks ? Ok(result.linkResponse.LinkedEntities) : Ok(result.linkResponse.ShapedEntities);
    }

    [HttpGet("{userId:guid}", Name = "UserById")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _service.UserService.GetUserAsync(userId: userId, trackChanges: false);
        return Ok(user);
    }

    [HttpPost(Name = "CreateUser")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateUser([FromBody] UserForCreationDto user)
    {
        var createdUser = await _service.UserService.CreateUserAsync(user);

        return CreatedAtRoute("UserById", new { userId = createdUser.Id }, createdUser);

    }

    [HttpGet("collection/({ids})", Name = "UserCollection")]
    public async Task<IActionResult> GetUserCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var users = await _service.UserService.GetByIdsAsync(ids, false);

        return Ok(users);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateUserCollection([FromBody] IEnumerable<UserForCreationDto> userCollection)
    {
        var result = await _service.UserService.CreateUserCollectionAsync(userCollection);

        return CreatedAtRoute("UserCollection", new { result.ids }, result.users);
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _service.UserService.DeleteUserAsync(userId, false);

        return NoContent();
    }

    [HttpPut("{userId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateUser([FromBody] UserForUpdateDto userForUpdateDto, Guid userId)
    {
        await _service.UserService.UpdateUserAsync(userForUpdateDto, userId, true);

        return NoContent();
    }


}