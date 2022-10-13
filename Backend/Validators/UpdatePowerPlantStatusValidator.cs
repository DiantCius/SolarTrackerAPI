using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class UpdatePowerplantStatusValidator : AbstractValidator<UpdatePowerplantStatusRequest>
    {
        public UpdatePowerplantStatusValidator()
        {
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
            RuleFor(x => x.ConnectionStatus).IsInEnum();
        }
    }
}
