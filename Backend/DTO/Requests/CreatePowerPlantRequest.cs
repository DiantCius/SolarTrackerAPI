using Backend.Models;

namespace Backend.DTO.Requests
{
    public record CreatePowerplantRequest(string Name,string City, string Tariff, PowerplantType PowerplantType, string SerialNumber);
}
