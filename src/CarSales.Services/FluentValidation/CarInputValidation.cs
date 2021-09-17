using CarSales.Services.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.FluentValidation
{
    public class CarInputValidation : AbstractValidator<CarInput>
    {
        public CarInputValidation()
        {

            RuleFor(x => x.VinCode)
                .NotEmpty()
                .Length(17);

            RuleFor(x => x.Brand)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Model)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.CarNumber)
                .NotEmpty()
                .MaximumLength(15);

            RuleFor(x => x.Price)
                .NotEmpty();

            RuleFor(x => x.ReleaseDate)
                .NotEmpty();

            RuleFor(x => x.StartedSale)
               .NotEmpty();

            RuleFor(x => x.FinishedSale)
               .NotEmpty();

        }
    }
}
