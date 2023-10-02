using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator() 
        {
            RuleFor(o => o.UserName)
                .NotEmpty().NotNull().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

            RuleFor(o => o.EmailAddress)
                .NotEmpty().NotNull().WithMessage("Email address is required.");

            RuleFor(o => o.TotalPrice)
                .NotEmpty().WithMessage("Total price is required.")
                .GreaterThan(50).WithMessage("Total price should be greater than zero.");
        }
    }
}
