using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class CreateCodeValidator : AbstractValidator<CreateCodeRequest>
    {
        public CreateCodeValidator()
        {
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
        }
    }
}
