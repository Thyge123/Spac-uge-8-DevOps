using CerealAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CerealAPI.Helpers
{
    public class AuthHelpers
    {
        private readonly IConfiguration _configuration;

        public AuthHelpers(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJWTToken(User user)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured")));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                var jwtToken = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }
            catch (Exception e)
            {
                throw new Exception("Error generating JWT token", e);
            }      
        }
    }
}