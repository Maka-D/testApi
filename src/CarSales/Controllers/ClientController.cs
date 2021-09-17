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
        private readonly IMemoryCache _memoryCache;

        public ClientController(IClientService clientService, IMemoryCache memoryCache)
        {
            _clientService = clientService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetClient(string IdentityNumber)
        {
            var cacheKey = IdentityNumber;
            if(! _memoryCache.TryGetValue(cacheKey, out Client client))
            {
                client = await _clientService.FindClient(IdentityNumber);
                var cacheEpiringOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };
                _memoryCache.Set(cacheKey, client, cacheEpiringOptions);
            }
                       
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

