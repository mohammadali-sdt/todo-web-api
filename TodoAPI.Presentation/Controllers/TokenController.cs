using System;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace TodoAPI.Presentation.Controllers;

public class TokenController : ControllerBase
{
    private readonly IServiceManager _service;

    public TokenController(IServiceManager service)
    {
        _service = service;
    }
}
