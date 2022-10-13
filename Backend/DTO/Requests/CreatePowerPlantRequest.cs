using Backend.Models;

namespace Backend.DTO.Requests
{
    public record CreatePowerplantRequest(string Name, string Location, PowerplantType PowerplantType, string SerialNumber);
}
