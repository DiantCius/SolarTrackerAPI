using Backend.Models;

namespace Backend.DTO.Requests
{
    public record CreatePowerPlantRequest(string Name, string Location, PowerPlantType PowerPlantType, string SerialNumber);
}
