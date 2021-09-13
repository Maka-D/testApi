using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.ValidateInput
{
    public interface IInputValidator
    {
        public bool IsValidIdentityNumber(string identityNum);
        public bool IsValidVinCode(string VinCode);

    }
}
