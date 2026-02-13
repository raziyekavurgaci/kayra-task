using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Application.Interfaces;
using Core.Entities;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // 1. Claims oluştur (kullanıcı bilgileri)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 2. Secret key al
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            // 3. İmza oluştur
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Expiration süresi
            var expirationHours = int.Parse(_configuration["Jwt:ExpirationHours"]!);
            var expiration = DateTime.UtcNow.AddHours(expirationHours);

            // 5. Token oluştur
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            // 6. String'e çevir
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
