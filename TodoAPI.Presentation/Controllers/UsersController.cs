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
    public IActionResult GetUsers()
    {
        var users = _service.UserService.GetAllUsers(trackChanges: false);

        return Ok(users);
    }

    [HttpGet("{userId:guid}", Name = "UserById")]
    public IActionResult GetUser(Guid userId)
    {
        var user = _service.UserService.GetUser(userId: userId, trackChanges: false);
        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] UserForCreationDto user)
    {
        if (user is null) return BadRequest("UserForCreationDto object is null");

        var createdUser = _service.UserService.CreateUser(user);

        return CreatedAtRoute("UserById", new { userId = createdUser.Id }, createdUser);
        
    }

    [HttpGet("collection/({ids})", Name = "UserCollection")]
    public IActionResult GetUserCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
    {
        var users = _service.UserService.GetByIds(ids, false);

        return Ok(users);
    }

    [HttpPost("collection")]
    public IActionResult CreateUserCollection([FromBody] IEnumerable<UserForCreationDto> userCollection)
    {
        var result = _service.UserService.CreateUserCollection(userCollection);

        return CreatedAtRoute("UserCollection", new { result.ids }, result.users);
    }
    
}