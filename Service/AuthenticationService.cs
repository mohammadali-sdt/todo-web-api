using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Contracs;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class AuthenticationService : ServiceBase, IAuthenticationService
{

    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    private User? _user;

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

    public async Task<bool> ValidateUser(UserForAuthenticateDto userForAuthenticateDto)
    {
        if (userForAuthenticateDto == null || string.IsNullOrWhiteSpace(userForAuthenticateDto.UserName) || string.IsNullOrWhiteSpace(userForAuthenticateDto.Password)) throw new Exception("userForAuthenticateDto is null");

        var userCredential = userForAuthenticateDto.UserName;

        _user = userCredential.Contains('@') ? await _userManager.FindByEmailAsync(userCredential) : await _userManager.FindByNameAsync(userCredential);

        if (_user == null)
            return false;

        var result = await _userManager.CheckPasswordAsync(_user, userForAuthenticateDto.Password);

        return result;

    }

    public async Task<string> CreateToken()
    {
        var singingCredentials = GetSigningCredentials();
        var claims = await GetUserClaims();

        return GenerateToken(singingCredentials, claims);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var envValue = Environment.GetEnvironmentVariable("TODOAPI_SECRET");
        if (envValue == null)
        {
            throw new Exception("Environment variable null value.");
        }
        var secretValue = Encoding.UTF8.GetBytes(envValue);
        var signingKey = new SymmetricSecurityKey(secretValue);

        return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetUserClaims()
    {
        if (_user == null || string.IsNullOrWhiteSpace(_user.UserName)) throw new Exception("cannot get user claims.");

        var claims = new List<Claim>(){
            new Claim(ClaimTypes.Name, _user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private string GenerateToken(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var expiresInMinutes = Convert.ToDouble(jwtSettings["expire"]);
        var expiresInDate = DateTime.Now.AddMinutes(expiresInMinutes);

        var jwtOptions = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
             issuer: jwtSettings["validIssuer"],
             audience: jwtSettings["validAudience"],
             expires: expiresInDate
             );

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(jwtOptions);
    }

}
