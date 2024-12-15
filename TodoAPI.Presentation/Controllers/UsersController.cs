using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using TodoAPI.Presentation.ModelBinder;

namespace TodoAPI.Presentation.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IServiceManager _service;

    public UsersController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _service.UserService.GetAllUsersAsync(trackChanges: false);

        return Ok(users);
    }

    [HttpGet("{userId:guid}", Name = "UserById")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _service.UserService.GetUserAsync(userId: userId, trackChanges: false);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserForCreationDto user)
    {
        if (user is null) return BadRequest("UserForCreationDto object is null");

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }

        var createdUser = await _service.UserService.CreateUserAsync(user);

        return CreatedAtRoute("UserById", new { userId = createdUser.Id }, createdUser);
        
    }

    [HttpGet("collection/({ids})", Name = "UserCollection")]
    public async Task<IActionResult> GetUserCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
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
    public async Task<IActionResult> UpdateUser ([FromBody] UserForUpdateDto userForUpdateDto, Guid userId)
    {
        await _service.UserService.UpdateUserAsync(userForUpdateDto, userId, true);

        return NoContent();
    }
    
    
}