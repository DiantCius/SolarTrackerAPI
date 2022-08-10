using Backend.DTO.Requests;
using FluentValidation;

namespace Backend.Validators
{
    public class DeletePowerPlantValidator : AbstractValidator<DeletePowerPlantRequest>
    {
        public DeletePowerPlantValidator()
        {
            RuleFor(x => x.SerialNumber).NotNull().NotEmpty();
        }
    }
}
