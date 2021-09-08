using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Token.TokenBasico.Models;

namespace Token.TokenBasico.Services
{
    public class JwtAuthenticationService : IJwtAuthenticationService
    {
        private readonly JwtTokenConfig _jwtTokenConfig;
        private readonly Dictionary<string, string> _lstUsers;

        public JwtAuthenticationService(JwtTokenConfig jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;

            _lstUsers = new Dictionary<string, string>()
            {
                {"admin", "admin"},
                {"user", "123456"}
            };
        }

        public string Authenticate(string username, string password)
        {
            var userExist = _lstUsers.Where(d => d.Key == username && d.Value == password).FirstOrDefault();

            if (!string.IsNullOrEmpty(userExist.Value))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_jwtTokenConfig.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Issuer = _jwtTokenConfig.Issuer,
                    Audience = _jwtTokenConfig.Audience,
                    Subject = new ClaimsIdentity(new Claim[]
                     {
                        new Claim(JwtRegisteredClaimNames.Sub, username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtClaimTypes.Role, "Admin")
                     }),
                    Expires = DateTime.Now.AddMinutes(15),
                    NotBefore = DateTime.Now,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }

            return string.Empty;
        }
    }
}
