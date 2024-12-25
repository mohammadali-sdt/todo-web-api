using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class AuthenticationService : ServiceBase, IAuthenticationService
{

    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration) : base(repositoryManager, logger, mapper)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUser(UserForRegisterationDto userForRegisterationDto)
    {

        // check for password and password confirm
        if (userForRegisterationDto.Password != userForRegisterationDto.PasswordConfirm)
        {
            throw new PasswordsDoesNotMatch();
        }

        if (userForRegisterationDto.Password is null)
        {
            throw new PasswordsDoesNotMatch();
        }

        // map userRegisterationDto to user model
        var user = Mapper.Map<User>(userForRegisterationDto);

        // Create User and save to DB
        var result = await _userManager.CreateAsync(user, userForRegisterationDto.Password);

        return result;
    }
}
