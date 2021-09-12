using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
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
            Client client;
            try
            {
                client = await _clientService.FindClient(IdentityNumber);               
            }
            catch(DoesNotExistsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest(ex);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }

            return Ok(client);

        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient(ClientInput client)
        {
            Client createdClient;
            try
            {
                createdClient = await _clientService.AddClient(client);
            }
            catch(AlreadyExistsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest(ex);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
            return CreatedAtAction(nameof(GetClient), new { Id = createdClient.Id, createdClient });
        }

        [HttpPut]
        public async Task<ActionResult> EditClient(ClientInput client)
        {
            try
            {
                await _clientService.UpdateClient(client);
               
            }
            catch (DoesNotExistsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }
            return Ok();

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteClient(string IdentityNumber)
        {
            try
            {
                await _clientService.DeleteClient(IdentityNumber);              
            }
            catch(Exception ex)
            {
                _logger.LogError(ex ,ex.Message);
                return BadRequest(ex);
            }
            return Ok();
            
        }
       
    }
}

