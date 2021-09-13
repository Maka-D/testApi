using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.ValidateInput
{
    public class InputValidator : IInputValidator
    {
        public bool IsValidIdentityNumber(string identityNum)
        {
            return (!string.IsNullOrEmpty(identityNum) && identityNum.Length == 11);
        }

        public bool IsValidVinCode(string VinCode)
        {
            return (!string.IsNullOrEmpty(VinCode) && VinCode.Length == 17);
        }
    }
}
