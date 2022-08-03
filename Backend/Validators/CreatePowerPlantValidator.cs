using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class CreatePowerPlantValidator : AbstractValidator<CreatePowerPlantRequest>
    {
        public CreatePowerPlantValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Location).NotNull().NotEmpty();
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
            RuleFor(x => x.PowerPlantType).IsInEnum();
        }
    }
}
