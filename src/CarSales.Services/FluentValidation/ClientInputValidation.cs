using CarSales.Services.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Services.FluentValidator
{
    public class ClientInputValidation : AbstractValidator<ClientInput>
    {
        public ClientInputValidation()
        {
            RuleFor(x => x.IdentityNumber)
                .NotEmpty()
                .Length(11);

            RuleFor(x => x.FirstName)
                .MaximumLength(20);

            RuleFor(x => x.SecondName)
                .MaximumLength(30);

            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.Address)
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty();
                
                
                
        }
    }
}
