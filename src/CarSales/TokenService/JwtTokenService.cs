using CarSales.Services.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.TokenService
{
    public class JwtTokenService  :ITokenService
    {
        private static IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public Task<string> GenerateJwtToken(LogInInput input)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, input.IdentityNumber)
            };

            var token = new JwtSecurityToken(_config["JWT:Issuer"], _config["JWT:Audience"],
                claims, 
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credintials);


            return  Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

    }
}
