using CarSales.Services.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.FluentValidation
{
    public class RegistrationInputValidation : AbstractValidator<RegistrationInput>
    {
        public RegistrationInputValidation()
        {
            RuleFor(x => x.IdentityNumber)
                .NotEmpty()
                .WithMessage("Identity Number Is Required!")
                .Length(11)
                .WithMessage("Identity Number must contain 11 characters!");

            RuleFor(x => x.FirstName)
                .MaximumLength(20);

            RuleFor(x => x.SecondName)
                .MaximumLength(30);

            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.Address)
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone Number Is Required!")
                .MaximumLength(15);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(20)
                .MinimumLength(6)
                .WithMessage("Password must contain");

            RuleFor(x => x.RepeatPassword)
                .NotEmpty()
                .MaximumLength(20)
                .MinimumLength(6)
                .Equal(x => x.Password);

        }

    }
}
