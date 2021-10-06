using CarSales.Domain.Models;
using CarSales.Services.ClientServices;
using CarSales.Services.DTOs;
using CarSales.TokenService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarSales.Controllers
{
    public class AccountController :ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _token;
        public AccountController(IUserService userService, ITokenService token)
        {
            _userService = userService;
            _token = token;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserInput client)
        {
            var createdClient = await _userService.AddClient(client);
            return Ok(createdClient);
        }

        [HttpPost]
        public IActionResult LogIn(LogInInput input)
        {
            
            var token = _token.GenerateJwtToken(input);
            return Ok(token);

        }
    }
}
