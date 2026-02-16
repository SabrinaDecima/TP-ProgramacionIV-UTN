using Application.Abstraction;
using Application.Abstraction.ExternalServices;
using Contracts.Login.Request;
using Contracts.User.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.ExternalServices;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPlanRepository _planRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthenticationService(IUserRepository userRepository, 
        IPlanRepository planRepository, IRoleRepository roleRepository,
        IConfiguration configuration,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _planRepository = planRepository;
        _roleRepository = roleRepository;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public string Login(LoginRequest request)
    {
        var user = _userRepository.GetByEmail(request.Email);

        if (user == null )
        {
            return string.Empty; 
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.Contraseña, request.Password);

        if(result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var roleName = user.Rol?.Nombre ?? "User"; 

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("UserId", user.Id.ToString())
        };

        // 4. Leer configuración JWT
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            throw new InvalidOperationException("JWT configuration is missing in appsettings.json.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    
}