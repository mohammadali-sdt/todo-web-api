using System;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using TodoAPI.Presentation.ActionFilters;

namespace TodoAPI.Presentation.Controllers;

[ApiController]
[Route("/api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _service;

    public AuthenticationController(IServiceManager serviceManager)
    {
        _service = serviceManager;
    }

    [HttpPost("register")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegisterationDto userForRegisterationDto)
    {

        var result = await _service.AuthenticationService.RegisterUser(userForRegisterationDto);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> LoginUser([FromBody] UserForAuthenticateDto userForAuthenticate)
    {

        var result = await _service.AuthenticationService.ValidateUser(userForAuthenticate);

        if (!result) return Unauthorized();

        var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

        return Ok(tokenDto);

    }
}
