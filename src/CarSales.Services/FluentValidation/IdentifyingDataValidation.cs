using CarSales.Services.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.FluentValidation
{
    public class IdentifyingDataValidation : AbstractValidator<IdentifyingData>
    {
        public IdentifyingDataValidation()
        {
            RuleFor(x => x.IdentityNumber)
                .NotEmpty()
                .Length(11);

            RuleFor(x => x.VinCode)
                .NotEmpty()
                .Length(17);
        }
    }
}
