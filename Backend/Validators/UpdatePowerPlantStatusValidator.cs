using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class UpdatePowerPlantStatusValidator : AbstractValidator<UpdatePowerPlantStatusRequest>
    {
        public UpdatePowerPlantStatusValidator()
        {
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
            RuleFor(x => x.ConnectionStatus).IsInEnum();
        }
    }
}
