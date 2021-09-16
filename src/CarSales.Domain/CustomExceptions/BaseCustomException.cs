using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.CustomExceptions
{
    public class BaseCustomException :Exception
    {
        public BaseCustomException() : base()
        {

        }
        public BaseCustomException(string message) : base(message)
        {

        }
    }
}
