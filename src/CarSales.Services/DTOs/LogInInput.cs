using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.DTOs
{
    public class LogInInput
    {
        public string IdentityNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; } 
          
    }  
}
