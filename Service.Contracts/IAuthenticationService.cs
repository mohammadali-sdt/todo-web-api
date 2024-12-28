using System;
using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegisterationDto userForRegisterationDto);
    Task<bool> ValidateUser(UserForAuthenticateDto userForAuthenticateDto);
    Task<TokenDto> CreateToken(bool populateExp);
}
