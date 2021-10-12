using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.ValidateInput
{
    public class InputValidator 
    {
        public static bool IsValidIdentityNumber(string identityNum)
        {
            return (!string.IsNullOrEmpty(identityNum) && identityNum.Length == 11);
        }

        public static bool IsValidVinCode(string VinCode)
        {
            return (!string.IsNullOrEmpty(VinCode) && VinCode.Length == 17);
        }

        
    }
}
