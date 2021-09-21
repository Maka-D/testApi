using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Repository.ErrorHandlerMiddleware;
using CarSales.Services.ClientServices;
using CarSales.Services.DTOs;
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
    public class ClientController : ControllerBase
    {
        
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClient(string IdentityNumber)
        {

            var client = await _clientService.FindClient(IdentityNumber);
            return Ok(client);

        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient(ClientInput client)
        {
            var createdClient = await _clientService.AddClient(client);
            return Ok(createdClient);
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

