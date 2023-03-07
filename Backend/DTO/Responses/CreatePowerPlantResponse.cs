using Backend.Models;

namespace Backend.DTO.Responses
{
    public record CreatePowerplantResponse(string Name, string City, double Latitude, double Longitude, string Tariff, PowerplantType PowerplantType, string SerialNumber, ConnectionStatus ConnectionStatus);
}
