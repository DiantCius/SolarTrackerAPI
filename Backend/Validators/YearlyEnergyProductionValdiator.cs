using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class YearlyEnergyProductionValdiator : AbstractValidator<YearlyEnergyProductionRequest>
    {
        public YearlyEnergyProductionValdiator()
        {
            RuleFor(x => x.serialNumber).NotNull().NotEmpty();
        }
    }

}
