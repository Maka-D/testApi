using CarSales.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarSales.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(ClientInput client);
    }
}
