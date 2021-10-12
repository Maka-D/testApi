using CarSales.Domain.Models;
using CarSales.Services.DTOs;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarSales.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateJwtAccessToken(Client input);
        Task<string> GenerateJwtRefreshToken();
        Task<bool> ValidateRefreshToken(string refreshToken);
    }
}
