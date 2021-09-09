using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.CustomExceptions
{
    public class CarDoesNotExistsException :Exception
    {
        public CarDoesNotExistsException() : base()
        {

        }
    }
}
