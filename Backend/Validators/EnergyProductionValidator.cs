using Backend.Models;
using FluentValidation;

namespace Backend.Validators
{
    public class EnergyProductionValidator : AbstractValidator<EnergyProduction>
    {
        public EnergyProductionValidator()
        {
            RuleFor(x => x.CurrentProduction).NotNull().NotEmpty();
            RuleFor(x => x.DailyProduction).NotNull().NotEmpty();
            RuleFor(x => x.CurrentTime).NotNull().NotEmpty();
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
        }
    }
}
