using AutoMapper;
using CarSales.Domain.Models;
using CarSales.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.MapperService
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<ClientInput, Client>();

            CreateMap<CarInput, Car>();           

        }
    }

  
}
