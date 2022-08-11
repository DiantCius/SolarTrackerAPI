using Backend.Models;

namespace Backend.DTO.Responses
{
    public record DeletePowerPlantResponse(string Name, string Location, PowerPlantType PowerPlantType, string SerialNumber, ConnectionStatus ConnectionStatus);
}
