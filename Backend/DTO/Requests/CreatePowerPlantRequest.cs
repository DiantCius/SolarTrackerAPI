using Backend.Models;

namespace Backend.DTO.Requests
{
    public record CreatePowerplantRequest(string Name,string City, double Latitude, double Longitude, string Tariff, PowerplantType PowerplantType, string SerialNumber);
}
