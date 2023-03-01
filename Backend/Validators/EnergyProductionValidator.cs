using Backend.DTO;
using FluentValidation;

namespace Backend.Validators
{
    public class EnergyProductionValidator : AbstractValidator<EnergyProductionDto>
    {
        public EnergyProductionValidator()
        {
            RuleFor(x => x.CurrentProduction).NotNull().NotEmpty();
            RuleFor(x => x.DailyProduction).NotNull().NotEmpty();
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
        }
    }
}
