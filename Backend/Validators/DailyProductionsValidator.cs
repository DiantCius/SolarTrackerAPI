using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class DailyProductionsValidator : AbstractValidator<DailyProductionsRequest>
    {
        public DailyProductionsValidator()
        {
            RuleFor(x => x.serialNumber).NotEmpty().NotNull();
            RuleFor(x => x.year).NotEmpty().NotNull();
            RuleFor(x => x.month).NotEmpty().NotNull();
            RuleFor(x => x.day).NotEmpty().NotNull();
        }
    }
}

