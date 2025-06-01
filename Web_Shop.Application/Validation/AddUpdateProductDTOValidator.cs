using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;

namespace Web_Shop.Application.Validation
{
    public class AddUpdateProductDTOValidator : AbstractValidator<AddUpdateProductDTO>
    {
        public AddUpdateProductDTOValidator()
        {
            RuleFor(request => request.Name).Length(3, 50).WithMessage("Pole 'Nazwa' należy podać w zakresie {MinLength} - {MaxLength} znaków");
            RuleFor(request => request.Description).Length(3, 255).WithMessage("Pole 'Opis' musi zawierać {MinLength} - {MaxLength} znaków");
            RuleFor(request => request.Price)
                .NotNull().WithMessage("Pole 'Cena' jest wymagane.") // Upewnia się, że Price nie jest null
                .GreaterThanOrEqualTo(0).WithMessage("Pole 'Cena' musi być liczbą nieujemną."); // Sprawdza, czy jest liczbą i większa lub równa 0

            RuleFor(request => request.Sku)
            .NotEmpty().WithMessage("Pole 'Sku' jest wymagane.")
            .Matches("^[A-Z]{3}_[A-Z]{3}_[A-Z0-9]{2,}$")
            .WithMessage("Pole 'Sku' musi być w formacie 'XXX_YYY_ZZZ01' (np. ELE_SMA_PUL05).");

        }
    }
}
