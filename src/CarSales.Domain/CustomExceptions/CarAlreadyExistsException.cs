using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.CustomExceptions
{
    public class CarAlreadyExistsException : BaseCustomException
    {
        public CarAlreadyExistsException() : base("Such car already exists!")
        {

        }

    }
}
