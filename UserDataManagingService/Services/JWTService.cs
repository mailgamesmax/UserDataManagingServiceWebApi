using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserDataManagingService.Models;

namespace UserDataManagingService.Services
{
    public class JWTService : IJWTService
    {
        public string GetJwtToken(string nameOrNickName, Role role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nameOrNickName),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var secretToken = _configuration["Jwt:Key"]; //appsettingse nurodytas jwt key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretToken)); //is kliento apdorotas key 

            var creds = new SigningCredentials(key, algorithm: SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //
        private readonly IConfiguration _configuration;
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
