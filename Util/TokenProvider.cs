using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TableTennisAPI.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TableTennisAPI.Util {
    public class TokenProvider(IConfiguration config) {
        public string Create(User user)
        {
            string secretKey = config["Appsettings:Token"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity([
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    ]),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = credentials,
                Issuer = config["AppSettings:issuer"],
                Audience = config["AppSettings:audience"]
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
