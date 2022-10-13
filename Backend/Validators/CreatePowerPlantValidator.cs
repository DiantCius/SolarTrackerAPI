using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class CreatePowerplantValidator : AbstractValidator<CreatePowerplantRequest>
    {
        public CreatePowerplantValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Location).NotNull().NotEmpty();
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
            RuleFor(x => x.PowerplantType).IsInEnum();
        }
    }
}
