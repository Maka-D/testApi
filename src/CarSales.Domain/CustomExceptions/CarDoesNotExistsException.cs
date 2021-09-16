using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.CustomExceptions
{
    public class CarDoesNotExistsException : BaseCustomException
    {
        public CarDoesNotExistsException() : base("Could not find such car!")
        {

        }

    }
}
