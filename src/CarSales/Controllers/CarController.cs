using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
using CarSales.Services.CarServices;
using CarSales.Services.DTOs;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CarController :ControllerBase
    {
        private readonly ICarService _carService;

        public CarController( ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCar(string IdentityNumber, CarInput car)
        {
            await _carService.AddCar(IdentityNumber, car);
            return Ok(car);
            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCar(IdentifyingData data)
        {

            await _carService.DeleteCar(data);          
            return Ok("Successfully Deleted");

        }

        [HttpPost("BuyCar")]
        public async Task<IActionResult> BuyCar(IdentifyingData data)
        {
            await _carService.BuyCar(data);
            return Ok();
        }

        [HttpPost("SellingCarsList")]
        [AllowAnonymous]
        public async Task<IActionResult> SellingCarsList(DateInput date)
        {
            var carsList = await _carService.SellingCarsList(date);
           
            return Ok(carsList.ToList());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Report()
        {
            var CarsListMonthly = await _carService.MonthlyReport();
            return Ok(CarsListMonthly.ToList());
        }
    }
}
