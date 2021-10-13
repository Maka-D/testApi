using CarSales.Services.ClientServices;
using CarSales.Services.DTOs;
using CarSales.Services.TokenService;
using CarSales.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarSales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IClientService _client;
        private readonly ITokenService _token;
        private readonly IUserService _userService;


        public AccountController(UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager
            , IClientService client
            , ITokenService token
            , IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _client = client;
            _token = token;
            _userService = userService;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(RegistrationInput input)
        {
            var user = new IdentityUser { UserName = input.IdentityNumber };
            var result = await _userManager.CreateAsync(user, input.Password);

            if (result.Succeeded)
            {
                var client = await _client.Register(input, user.Id);
                return Ok(client);
            }

            return BadRequest(result.Errors.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInInput input)
        {
            var user = await _userManager.FindByNameAsync(input.IdentityNumber);
            var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, false);

            if (result.Succeeded)
            {
                var client = await _client.FindClient(input.IdentityNumber);
                var accessToken = await _token.GenerateJwtAccessToken(client);
                var refreshToken = await _token.GenerateJwtRefreshToken();
                await _userManager.SetAuthenticationTokenAsync(user, "JwtBearer", "Access Token", accessToken);
                await _userManager.SetAuthenticationTokenAsync(user, "JwtBearer", "Refresh Token", refreshToken);
                return Ok(new { client, accessToken, refreshToken });
            }

            return BadRequest();
        }

        [HttpPost("RefreshAccesToken")]
        public async Task<IActionResult> RefreshAccesToken(string refreshToken)
        {
            var user = await _userService.GetUserByRefreshToken(refreshToken);
            var client = await _client.FindClient(user.UserName);
            var accessToken = await _token.GenerateJwtAccessToken(client);

            await _userManager.SetAuthenticationTokenAsync(user, "JwtBearer", "Access Token", accessToken);

            return Ok(new { accessToken, refreshToken });

        }

        [HttpDelete("RemoveRefreshToken")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveRefreshToken()
        {
            var userIdentityNumber = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdentityNumber == null)
                return Unauthorized();

            var user = await _userManager.FindByNameAsync(userIdentityNumber.Value);

            await _userManager.RemoveAuthenticationTokenAsync(user, "JwtBearer", "Refresh Token");

            return NoContent();
        }


    }
}
