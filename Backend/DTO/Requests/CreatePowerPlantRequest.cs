using Backend.Models;

namespace Backend.DTO.Requests
{
    public record CreatePowerplantRequest(string Name, PowerplantType PowerplantType, string SerialNumber);
}
