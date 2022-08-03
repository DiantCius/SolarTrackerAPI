using Backend.Models;

namespace Backend.DTO.Responses
{
    public record CreatePowerPlantResponse(string Name, string Location, PowerPlantType PowerPlantType);
}
