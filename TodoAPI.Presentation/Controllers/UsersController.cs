using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

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

    [HttpGet("{userId:guid}")]
    public IActionResult GetUser(Guid userId)
    {
        var user = _service.UserService.GetUser(userId: userId, trackChanges: false);
        return Ok(user);
    }
}