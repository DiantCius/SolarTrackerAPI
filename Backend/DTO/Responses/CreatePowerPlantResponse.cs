using Backend.Models;

namespace Backend.DTO.Responses
{
    public record CreatePowerplantResponse(string Name, string Location, PowerplantType PowerplantType, string SerialNumber, ConnectionStatus ConnectionStatus);
}
