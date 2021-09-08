using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.CustomExceptions
{
    class ClientAlreadyExistsException :Exception
    {
        public ClientAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
