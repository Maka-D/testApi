﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.CustomExceptions
{
    public class ClientDoesNotExistsException : Exception
    {
        public ClientDoesNotExistsException() : base("Coudn't find the client with this Identity Number!")
        {

        }
    }
}
