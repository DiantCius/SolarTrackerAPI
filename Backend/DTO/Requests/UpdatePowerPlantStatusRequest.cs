using Backend.Models;

namespace Backend.DTO.Requests
{
    public record UpdatePowerplantStatusRequest(string SerialNumber, ConnectionStatus ConnectionStatus);
}
