using CarSales.Services.ClientService;
using CarSales.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
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
        
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;

        public ClientController(ILogger<ClientController> logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClient(string IdentityNumber)
        {
            var client = await _clientService.GetClient(IdentityNumber);
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient(ClientInput client)
        {
            var createdClient =  await _clientService.AddClient(client);
            return CreatedAtAction(nameof(GetClient), new { Id = createdClient.Id, createdClient });
        }

       
    }
}

