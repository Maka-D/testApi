﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.CustomExceptions
{
    public class ClientAlreadyExistsException :Exception
    {
        public ClientAlreadyExistsException() : base("Such client already exists!")
        {

        }
    }
}
