using CarSales.Domain.CustomExceptions;
using CarSales.Domain.Models;
using CarSales.Domain.Models.ReportModel;
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
    public class CarController :Controller
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
            try
            {
                await _carService.AddCar(IdentityNumber, car);
            }
            catch (AlreadyExistsException e)
            {
                _logger.LogWarning(e, e.Message);
                return BadRequest();
            }
            catch (DoesNotExistsException e)
            {
                _logger.LogWarning(e, e.Message);
                return BadRequest();
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            return Ok(car);
            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCar(string IdentityNumber, string VinCode)
        {
            try
            {
                await _carService.DeleteCar(IdentityNumber, VinCode);
            }
            catch (DoesNotExistsException e)
            {
                _logger.LogWarning(e, e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
            return Ok();

        }

        [HttpPost("BuyCar")]
        public async Task<IActionResult> BuyCar(string IdentityNumber, string VinCode)
        {
            try
            {
                await _carService.BuyCar(IdentityNumber, VinCode);
            }
            catch(CouldNotBuyCarException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest();
            }
            catch(DoesNotExistsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("SellingCarsList")]
        public async Task<IActionResult> SellingCarsList(DateTime from, DateTime to)
        {
            IEnumerable<Car> CarsList;
            try
            {
                CarsList = await _carService.SellingCarsList(from, to);
            }
            catch (DoesNotExistsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            return Ok(CarsList);
        }

        [HttpGet]
        public async Task<IActionResult> Report()
        {
            List<ReportData> CarsListMonthly;
            try
            {
                CarsListMonthly = await _carService.MonthlyReport();
            }
            catch (DoesNotExistsException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            return Ok(CarsListMonthly);
        }
    }
}
