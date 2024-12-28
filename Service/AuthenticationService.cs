using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

    public AuthenticationService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper,
        UserManager<User> userManager, IConfiguration configuration) : base(repositoryManager, logger, mapper)
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
        if (userForAuthenticateDto == null || string.IsNullOrWhiteSpace(userForAuthenticateDto.UserName) ||
            string.IsNullOrWhiteSpace(userForAuthenticateDto.Password))
            throw new Exception("userForAuthenticateDto is null");

        var userCredential = userForAuthenticateDto.UserName;

        _user = userCredential.Contains('@')
            ? await _userManager.FindByEmailAsync(userCredential)
            : await _userManager.FindByNameAsync(userCredential);


        var result = _user != null && await _userManager.CheckPasswordAsync(_user, userForAuthenticateDto.Password);

        if (!result)
            Logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong username or password");

        return result;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var singingCredentials = GetSigningCredentials();
        var claims = await GetUserClaims();
        var tokenOptions = GenerateTokenOptions(singingCredentials, claims);
        var tokenHandler = new JwtSecurityTokenHandler();
        var refreshToken = GenerateRefreshToken();
        _user.RefreshToken = refreshToken;
        if (populateExp)
        {
            _user.RefreshTokenExpiration = DateTime.Now.AddDays(7);
        }

        await _userManager.UpdateAsync(_user);

        var accessToken = tokenHandler.WriteToken(tokenOptions);
        var tokenDto = new TokenDto() { AccessToken = accessToken, RefreshToken = refreshToken };
        return tokenDto;
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var environmentVar = Environment.GetEnvironmentVariable("SECRET_TOKEN_KEY");

        if (string.IsNullOrWhiteSpace(environmentVar)) throw new Exception("Environment Variable is null");

        var secretValue = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(environmentVar));
        var tokenValidationParameter = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secretValue,
            ValidateLifetime = false,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidAudience = jwtSettings["validAudience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;

    }

    private SigningCredentials GetSigningCredentials()
    {
        var envValue = Environment.GetEnvironmentVariable("SECRET_TOKEN_KEY");
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

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
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

        return jwtOptions;
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {

        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiration <= DateTime.Now)
            throw new RefreshTokenBadRequest();

        _user = user;

        return await CreateToken(populateExp: false);

    }
}