using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class MonthlyProductionValidator : AbstractValidator<MonthlyProductionsRequest>
    {
        public MonthlyProductionValidator() { 
            RuleFor(x=>x.serialNumber).NotEmpty().NotNull();
            RuleFor(x => x.year).NotEmpty().NotNull();
            RuleFor(x => x.month).NotEmpty().NotNull();
        }
    }
}
