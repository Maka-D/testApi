using CarSales.Domain.Models;
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

namespace CarSales.Services.TokenService
{
    public class JwtTokenService  :ITokenService
    {
        private static IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public Task<string> GenerateJwtAccessToken(Client input)
        {           
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, input.IdentityNumber)
            };

            var token = GenerateJwtToken(_config["JWT:Issuer"], _config["JWT:Audience"], _config["JWT:AccessKey"], 3, claims);

            return token;
        }

        public Task<string> GenerateJwtRefreshToken()
        {
            var token = GenerateJwtToken(_config["JWT:Issuer"], _config["JWT:Audience"], _config["JWT:RefreshKey"], 720);

            return token;
        }

        private static Task<string> GenerateJwtToken(string issuer, string audience, string secretKey, double expirationTimeInHours, IEnumerable<Claim> claims = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(issuer, audience, claims,
                expires: DateTime.Now.AddHours(expirationTimeInHours),
                signingCredentials: credintials);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public Task<bool> ValidateRefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentNullException("Refresh Token is Required!");

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidIssuer = _config["JWT:Issuer"],
                ValidAudience = _config["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:RefreshKey"])),
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            _ = tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validToken);

            return Task.FromResult(true);

        }
    }
}
