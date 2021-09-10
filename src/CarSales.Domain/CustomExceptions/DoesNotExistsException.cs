using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.CustomExceptions
{
    public class DoesNotExistsException :Exception
    {
        public DoesNotExistsException() : base()
        {

        }

        public DoesNotExistsException(string message) : base(message)
        {

        }
    }
}
