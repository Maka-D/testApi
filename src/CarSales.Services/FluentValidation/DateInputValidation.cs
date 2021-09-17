using CarSales.Services.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.FluentValidation
{
    public class DateInputValidation : AbstractValidator<DateInput>
    {
        public DateInputValidation()
        {
            RuleFor(x => x.From).NotEmpty().WithMessage("Date Is Required!");

            RuleFor(x => x.To).NotEmpty().GreaterThan(x => x.From).WithMessage("Second Date Must be Greater than first Date!");
        }

    }
}
