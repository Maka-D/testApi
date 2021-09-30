using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository.ErrorHandlerMiddleware;
using CarSales.Services.ClientServices;
using CarSales.Services.DTOs;
using CarSales.TokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarSales.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        
        private readonly IClientService _clientService;
        private readonly ITokenService _token;

        public ClientController(IClientService clientService, ITokenService token)
        {
            _clientService = clientService;
            _token = token;
        }

        [HttpGet]
        public async Task<IActionResult> GetClient(string IdentityNumber)
        {
            var client = await _clientService.FindClient(IdentityNumber);
            return Ok(client);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClient(ClientInput client)
        {
            var createdClient = await _clientService.AddClient(client);
            //if(!_token.IsValidToken(client))
             var token =   await _token.GenerateJwtToken(client);
            return Ok(new { createdClient , token});
        }

        [HttpPut]
        public async Task<ActionResult> EditClient(ClientInput client)
        {
            await _clientService.UpdateClient(client);
            return Ok();

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteClient(string IdentityNumber)
        {
            await _clientService.DeleteClient(IdentityNumber);
            return Ok();
            
        }
       
    }
}

