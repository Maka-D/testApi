using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.CustomExceptions
{
    public class ClientDoesNotExistsException : BaseCustomException
    {
        public ClientDoesNotExistsException() : base("Coud not find such client!")
        {

        }
    }
}
