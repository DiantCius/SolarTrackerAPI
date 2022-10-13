using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class DeletePowerplantValidator : AbstractValidator<DeletePowerplantRequest>
    {
        public DeletePowerplantValidator()
        {
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
        }
    }
}
