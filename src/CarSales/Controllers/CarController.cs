using CarSales.Services.CarService;
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
    public class CarController :ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly ICarService _carService;

        public CarController(ILogger<ClientController> logger, ICarService carService)
        {
            _logger = logger;
            _carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCar(string IdentityNumber, CarInput car)
        {
            await _carService.AddCar(IdentityNumber, car);
            return Ok();
        }

    }
}
