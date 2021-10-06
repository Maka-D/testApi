using CarSales.Services.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.FluentValidation
{
    public class LogInInputValidation :AbstractValidator<LogInInput>
    {
        public LogInInputValidation()
        {
            RuleFor(x => x.IdentityNumber)
                .NotEmpty()
                .WithMessage("Identity Number Is Required!")
                .Length(11)
                .WithMessage("Identity Number must contain 11 characters");

            RuleFor(x => x.Password)
               .NotEmpty()
               .WithMessage("Password Is Required!")
               .MaximumLength(20);
        }
    }
}
