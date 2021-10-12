using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository.ErrorHandlerMiddleware;
using CarSales.Services.ClientServices;
using CarSales.Services.DTOs;
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
    [Authorize(AuthenticationSchemes ="Bearer")]
    public class ClientController : ControllerBase
    {
        
        private readonly IClientService _userService;

        public ClientController(IClientService clientService)
        {
            _userService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClient(string IdentityNumber)
        {
            var client = await _userService.FindClient(IdentityNumber);
            return Ok(client);

        }


        [HttpPut]
        public async Task<ActionResult> EditClient(ClientInput client)
        {
            await _userService.UpdateClient(client);
            return Ok();

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteClient(string IdentityNumber)
        {
            await _userService.DeleteClient(IdentityNumber);
            return Ok();
            
        }
       
    }
}

